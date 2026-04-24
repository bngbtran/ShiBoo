using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using ShiBoo.Data;
using ShiBoo.Models;

namespace ShiBoo.Views.Member
{
    public partial class PersonalShift : UserControl
    {
        public int CurrentUserId { get; set; }

        public PersonalShift()
        {
            InitializeComponent();
        }

        // Load danh sách ca trực cá nhân
        public void LoadMyShifts()
        {
            using var db = new ShiBooDbContext();
            var list = db.Shifts.Where(s => s.UserId == CurrentUserId).ToList();
            dgMyShifts.ItemsSource = list;
        }

        // Hàm xử lý hiển thị khi ComboBox được load lên
        private void cbNewShift_Loaded(object sender, RoutedEventArgs e)
        {
            var cb = sender as ComboBox;
            var shift = cb?.DataContext as ShiBoo.Models.Shift;

            if (shift != null)
            {
                // Ưu tiên hiển thị ca mới đang chờ trong Note, nếu không có thì hiện ShiftName
                if (!string.IsNullOrEmpty(shift.Note))
                {
                    cb.SelectedValue = shift.Note;
                }
                else
                {
                    cb.SelectedValue = shift.ShiftName;
                }
            }
        }

        // Gửi yêu cầu đăng ký ca mới hoàn toàn
        private void BtnRequest_Click(object sender, RoutedEventArgs e)
        {
            if (dpRequestDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày!");
                return;
            }

            using var db = new ShiBooDbContext();
            var newRequest = new Shift
            {
                UserId = CurrentUserId,
                Date = dpRequestDate.SelectedDate.Value,
                ShiftName = (cbRequestShift.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Ca 1",
                Status = "Pending",
                RequestType = "MemberRequest",
                Note = ""
            };

            db.Shifts.Add(newRequest);
            db.SaveChanges();
            LoadMyShifts();
            MessageBox.Show("Đã gửi yêu cầu đăng ký!");
        }

        // Gửi yêu cầu sửa ca đã có (Đổi ca)
        private void BtnEditRequest_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var shift = button?.DataContext as ShiBoo.Models.Shift;
            if (shift == null) return;

            // Tìm ComboBox cùng hàng thông qua VisualTree
            var parent = VisualTreeHelper.GetParent(button) as FrameworkElement;
            while (parent != null && !(parent is StackPanel))
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;

            if (parent is StackPanel panel)
            {
                var cb = panel.Children.OfType<ComboBox>().FirstOrDefault(c => c.Name == "cbNewShift");
                if (cb != null && cb.SelectedItem is ComboBoxItem selectedItem)
                {
                    string newShiftName = selectedItem.Content.ToString();

                    // Chống gửi trùng ca cũ
                    if (newShiftName == shift.ShiftName)
                    {
                        MessageBox.Show("Vui lòng chọn ca khác với ca hiện tại!");
                        return;
                    }

                    using var db = new ShiBooDbContext();
                    var item = db.Shifts.Find(shift.Id);
                    if (item != null)
                    {
                        item.Status = "Change_Request";
                        item.Note = newShiftName; // Lưu ca mới vào Note để chờ Admin duyệt

                        db.SaveChanges();
                        LoadMyShifts(); // Refresh lại bảng, lúc này cbNewShift_Loaded sẽ tự bắt lấy Note để hiển thị
                        MessageBox.Show($"Đã gửi yêu cầu đổi sang {newShiftName}!");
                    }
                }
            }
        } // Kết thúc hàm BtnEditRequest_Click
    } // Kết thúc class PersonalShift
} // Kết thúc namespace