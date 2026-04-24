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

    var memberList = db.Users.Where(u => u.Role == "Member").ToList();
    cbUserSuggest.ItemsSource = null; 
    cbUserSuggest.ItemsSource = memberList;

    var allUsers = db.Users.ToList();
    var shifts = db.Shifts.ToList();

    foreach (var s in shifts)
    {
        var u = allUsers.FirstOrDefault(x => x.Id == s.UserId);
        s.UserName = u != null ? u.Name : "Nhân viên đã bị xóa";
    }

    dgPendingShifts.ItemsSource = null; 
    dgPendingShifts.ItemsSource = shifts
        .Where(x => x.Status == "Pending" || x.Status == "Change_Request")
        .OrderByDescending(x => x.Date) 
        .ToList();

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
            if (item.Status == "Change_Request" && !string.IsNullOrEmpty(item.Note))
            {
                item.ShiftName = item.Note; 
                item.Note = ""; 
            }
            
            item.Status = "Approved";
            db.SaveChanges();
            RefreshData(); 
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
                item.Status = "Approved"; 
                item.Note = "";           
                MessageBox.Show("Đã từ chối yêu cầu đổi. Ca cũ được giữ nguyên.");
            }
            else
            {
                db.Shifts.Remove(item);
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
            Dispatcher.BeginInvoke(new Action(() =>
            {
                using var db = new ShiBooDbContext();
                var item = db.Shifts.Find(shift.Id);
                
                if (item != null)
                {
                    item.ShiftName = shift.ShiftName;

                    db.SaveChanges();

                    RefreshData(); 
                }
            }), System.Windows.Threading.DispatcherPriority.Background);
        }
    }
}
    }
} 