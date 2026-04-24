using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ShiBoo.Data;
using System.Windows.Threading;
using ShiBoo.Models;

namespace ShiBoo.Views.Admin
{
    public partial class ShiftApproval : UserControl
    {
        public ShiftApproval()
        {
            InitializeComponent();
            RefreshData();
        }

        private void RefreshData()
{
    using var db = new ShiBooDbContext();

    // 1. Cập nhật ComboBox gợi ý
    var memberList = db.Users.Where(u => u.Role == "Member").ToList();
    cbUserSuggest.ItemsSource = null; 
    cbUserSuggest.ItemsSource = memberList;

    // 2. Lấy dữ liệu mới nhất từ Database
    // Sử dụng .AsNoTracking() để đảm bảo lấy dữ liệu thực tế từ DB, không lấy từ cache của EF Core
    var allUsers = db.Users.ToList();
    var shifts = db.Shifts.ToList();

    foreach (var s in shifts)
    {
        var u = allUsers.FirstOrDefault(x => x.Id == s.UserId);
        s.UserName = u != null ? u.Name : "Nhân viên đã bị xóa";
    }

    // 3. Cập nhật DataGrid (Ép UI reset bằng cách gán null trước)
    
    // Đơn chờ duyệt
    dgPendingShifts.ItemsSource = null; 
    dgPendingShifts.ItemsSource = shifts
        .Where(x => x.Status == "Pending" || x.Status == "Change_Request")
        .OrderByDescending(x => x.Date) // Sắp xếp ngày mới nhất lên đầu cho dễ duyệt
        .ToList();

    // Tất cả ca trực
    dgAllShifts.ItemsSource = null;
    dgAllShifts.ItemsSource = shifts.OrderByDescending(x => x.Date).ToList();
}

        private void BtnApprove_Click(object sender, RoutedEventArgs e)
{
    if ((sender as Button)?.DataContext is Shift shift)
    {
        using var db = new ShiBooDbContext();
        var item = db.Shifts.Find(shift.Id);
        
        if (item != null)
        {
            // Nếu là yêu cầu đổi ca
            if (item.Status == "Change_Request" && !string.IsNullOrEmpty(item.Note))
            {
                // Chính thức đổi ca cũ thành ca mới
                item.ShiftName = item.Note; 
                item.Note = ""; // Xóa yêu cầu cũ
            }
            
            item.Status = "Approved"; // Chuyển trạng thái thành đã duyệt
            db.SaveChanges();
            RefreshData(); // Load lại bảng
            MessageBox.Show("Đã phê duyệt thay đổi ca!");
        }
    }
}

       private void BtnReject_Click(object sender, RoutedEventArgs e)
{
    if ((sender as Button)?.DataContext is Shift shift)
    {
        using var db = new ShiBooDbContext();
        var item = db.Shifts.Find(shift.Id);
        
        if (item != null)
        {
            if (item.Status == "Change_Request")
            {
                item.Status = "Approved"; // Giữ lại ca trực cũ
                item.Note = "";           // Xóa yêu cầu đổi ca đi
                MessageBox.Show("Đã từ chối yêu cầu đổi. Ca cũ được giữ nguyên.");
            }
            else
            {
                db.Shifts.Remove(item); // Nếu là đăng ký mới (Pending) thì xóa hẳn
                MessageBox.Show("Đã xóa yêu cầu đăng ký mới.");
            }
            
            db.SaveChanges();
            RefreshData();
        }
    }
}

        private void BtnAssign_Click(object sender, RoutedEventArgs e)
        {
            if (cbUserSuggest.SelectedValue == null || dpAssignDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn Nhân viên và Ngày!");
                return;
            }

            int userId = (int)cbUserSuggest.SelectedValue;

            using var db = new ShiBooDbContext();
            var newShift = new Shift
            {
                UserId = userId,
                Date = dpAssignDate.SelectedDate.Value,
                ShiftName = (cbAssignShift.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Ca 1",
                Status = "Approved",
                RequestType = "AdminAssign"
            };

            db.Shifts.Add(newShift);
            db.SaveChanges();

            cbUserSuggest.SelectedIndex = -1;
            RefreshData();
            MessageBox.Show("Gán ca thành công!");
        }

        private void BtnDeleteShift_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is Shift shift)
            {
                using var db = new ShiBooDbContext();
                var item = db.Shifts.Find(shift.Id);
                if (item != null)
                {
                    db.Shifts.Remove(item);
                    db.SaveChanges();
                    RefreshData();
                }
            }
        }

        private void dgAllShifts_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
{
    if (e.EditAction == DataGridEditAction.Commit)
    {
        var shift = e.Row.Item as ShiBoo.Models.Shift;
        if (shift != null)
        {
            // Đợi UI cập nhật giá trị mới vào object shift xong mới lưu
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using var db = new ShiBooDbContext();
                var item = db.Shifts.Find(shift.Id);
                
                if (item != null)
                {
                    // 1. Cập nhật tên ca mới
                    item.ShiftName = shift.ShiftName;

                    // 2. (Tùy chọn) Nếu muốn đơn này hiện lại ở bảng chờ duyệt sau khi sửa:
                    // item.Status = "Pending"; 

                    db.SaveChanges();

                    // 3. QUAN TRỌNG: Gọi hàm này để cả 2 bảng cùng load lại dữ liệu mới nhất từ DB
                    RefreshData(); 
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
    } // Kết thúc class
} // Kết thúc namespace (Cái này là cái bạn thiếu)