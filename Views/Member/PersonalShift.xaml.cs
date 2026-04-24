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

        public void LoadMyShifts()
        {
            using var db = new ShiBooDbContext();
            var list = db.Shifts.Where(s => s.UserId == CurrentUserId).ToList();
            dgMyShifts.ItemsSource = list;
        }

        private void cbNewShift_Loaded(object sender, RoutedEventArgs e)
        {
            var cb = sender as ComboBox;
            var shift = cb?.DataContext as ShiBoo.Models.Shift;

            if (shift != null)
            {
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

        private void BtnEditRequest_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var shift = button?.DataContext as ShiBoo.Models.Shift;
            if (shift == null) return;

            var parent = VisualTreeHelper.GetParent(button) as FrameworkElement;
            while (parent != null && !(parent is StackPanel))
                parent = VisualTreeHelper.GetParent(parent) as FrameworkElement;

            if (parent is StackPanel panel)
            {
                var cb = panel.Children.OfType<ComboBox>().FirstOrDefault(c => c.Name == "cbNewShift");
                if (cb != null && cb.SelectedItem is ComboBoxItem selectedItem)
                {
                    string newShiftName = selectedItem.Content.ToString();

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
                        item.Note = newShiftName; 

                        db.SaveChanges();
                        LoadMyShifts(); 
                        MessageBox.Show($"Đã gửi yêu cầu đổi sang {newShiftName}!");
                    }
                }
            }
        } 
    } 
} 