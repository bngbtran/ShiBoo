using System.Linq;
using System.Windows;
using System.Windows.Controls;
using ShiBoo.Data;

namespace ShiBoo.Views.Member
{
    public partial class ChangePassword : UserControl
    {
        // Giả sử ta truyền Email vào để biết đổi cho ai
        public string UserEmail { get; set; } = "";

        public ChangePassword()
        {
            InitializeComponent();
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
           if (string.IsNullOrWhiteSpace(txtNewPass.Password))
    {
        MessageBox.Show("Vui lòng nhập mật khẩu!");
        return;
    }

            using var db = new ShiBooDbContext();
            var user = db.Users.FirstOrDefault(u => u.Email == UserEmail);
            if (user != null)
            {
                user.Password = txtNewPass.Password;
                user.IsFirstLogin = false;
                db.SaveChanges();

                MessageBox.Show("Đổi mật khẩu thành công! Vui lòng đăng nhập lại.");
                MainWindow.Instance?.NavigateTo(new LoginView());
            }
        }
    }
}