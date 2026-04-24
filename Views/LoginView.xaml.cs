using System.Windows;
using System.Windows.Controls;
using ShiBoo.ViewModels;

namespace ShiBoo.Views
{
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            var vm = (LoginViewModel)DataContext;

            // 🔥 lấy password từ UI
            vm.Password = txtPassword.Password;

            vm.Email = vm.Email?.Trim();
            vm.Password = vm.Password?.Trim();

            vm.LoginCommand.Execute(null);
        }
    }
}