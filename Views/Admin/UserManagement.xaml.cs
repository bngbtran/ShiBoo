// D:\ShiBoo\Views\Admin\UserManagement.xaml.cs
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ShiBoo.Data;
using ShiBoo.Models;

namespace ShiBoo.Views.Admin
{
    public partial class UserManagement : UserControl
    {
        public UserManagement()
        {
            InitializeComponent();
            LoadData();
        }

        // Hàm tải danh sách từ DB lên bảng
        private void LoadData()
        {
            using var db = new ShiBooDbContext();
            dgUsers.ItemsSource = db.Users.ToList();
        }

        // Xử lý Thêm
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) || string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!");
                return;
            }

            using var db = new ShiBooDbContext();
            
            // Kiểm tra email trùng
            if (db.Users.Any(u => u.Email == txtEmail.Text))
            {
                MessageBox.Show("Email này đã tồn tại!");
                return;
            }

            var newUser = new User
            {
                Name = txtName.Text,
                Email = txtEmail.Text,
                Role = (cbRole.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Member",
                Password = "123@abc", // Mật khẩu mặc định
                IsFirstLogin = true
            };

            db.Users.Add(newUser);
            db.SaveChanges();
            
            // Reset form và load lại
            txtName.Clear();
            txtEmail.Clear();
            LoadData();
            MessageBox.Show("Thêm nhân viên thành công!");
        }

        // Xử lý Xóa
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var user = button?.DataContext as User;

            if (user != null)
            {
                if (user.Role == "Admin")
                {
                    MessageBox.Show("Không thể xóa tài khoản Admin hệ thống!");
                    return;
                }

                var result = MessageBox.Show($"Bạn có chắc muốn xóa {user.Name}?", "Xác nhận", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    using var db = new ShiBooDbContext();
                    db.Users.Remove(user);
                    db.SaveChanges();
                    LoadData();
                }
            }
        }

        private void BtnGoToShifts_Click(object sender, RoutedEventArgs e)
{
    // Gọi Instance của MainWindow để điều hướng sang trang ShiftApproval
    MainWindow.Instance?.NavigateTo(new ShiBoo.Views.Admin.ShiftApproval());
}
    }
}