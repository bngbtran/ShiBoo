using System.Linq;
using System.Windows;
using ShiBoo.Data;
using ShiBoo.Helpers;
using ShiBoo.Models;

namespace ShiBoo.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string _email = "";
        public string Email { get => _email; set { _email = value; OnPropertyChanged(); } }

        private string _password = "";
        public string Password { get => _password; set { _password = value; OnPropertyChanged(); } }

        public RelayCommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private void ExecuteLogin(object? parameter)
{
    using var db = new ShiBooDbContext();
    var user = db.Users.FirstOrDefault(u => u.Email == Email && u.Password == Password);

    if (user != null)
    {
        MainWindow.Instance?.LoginSuccess(user);
    }
    else
    {
        MessageBox.Show("Sai email hoặc mật khẩu!");
    }
}
    }
}