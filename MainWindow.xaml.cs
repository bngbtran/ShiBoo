using System;
using System.Windows;
using System.Windows.Controls;
using ShiBoo.Views;
using ShiBoo.Views.Admin;
using ShiBoo.Views.Member;

namespace ShiBoo
{
    public partial class MainWindow : Window
    {
        public static MainWindow? Instance { get; private set; }
        
        private ShiBoo.Models.User? _currentUser;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            ShowLogin();
        }

        public void ShowLogin()
        {
            _currentUser = null; 
            NavBar.Visibility = Visibility.Collapsed;
            MainContent.Content = new LoginView();
        }

        public void LoginSuccess(ShiBoo.Models.User user)
        {
            _currentUser = user; 
            NavBar.Visibility = Visibility.Visible;
            
            if (user.Role == "Admin")
            {
                NavigateTo(new UserManagement());
            }
            else
            {
                if (user.IsFirstLogin)
                {
                    NavigateTo(new ChangePassword { UserEmail = user.Email });
                }
                else
                {
                    var psView = new PersonalShift { CurrentUserId = user.Id };
                    NavigateTo(psView);
                    psView.LoadMyShifts();
                }
            }
        }

        public void NavigateTo(UserControl view)
        {
            MainContent.Content = view;

            if (_currentUser?.Role == "Admin" && view is ShiftApproval)
            {
                BtnBack.Visibility = Visibility.Visible;
            }
            else
            {
                BtnBack.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn đăng xuất?", "Xác nhận", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ShowLogin();
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            NavigateTo(new UserManagement());
        }
    }
}