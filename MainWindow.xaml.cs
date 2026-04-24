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
        
        // Chuyển biến này vào bên trong Class
        private ShiBoo.Models.User? _currentUser;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            ShowLogin();
        }

        public void ShowLogin()
        {
            _currentUser = null; // Xóa user khi về login
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

            // TỰ ĐỘNG KIỂM TRA HIỂN THỊ NÚT BACK
            // Chỉ hiện khi: User là Admin VÀ Trang hiện tại là ShiftApproval
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
            // Quay lại trang quản lý nhân sự
            NavigateTo(new UserManagement());
        }
    }
}