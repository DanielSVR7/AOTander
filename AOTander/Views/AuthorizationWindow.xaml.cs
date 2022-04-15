using AOTander.Models;
using AOTander.ViewModels;
using AOTander.Views;
using System;
using System.Windows;
using System.Windows.Controls;

namespace AOTander.Views
{
    public partial class AuthorizationWindow : Window
    {
        public AuthorizationWindow()
        {
            InitializeComponent();
            AuthorizationWindowViewModel vm = new AuthorizationWindowViewModel();
            this.DataContext = vm;
            if (vm.LoginAction == null)
                vm.LoginAction = new Action(() => Login(vm.AuthorizedUser));
        }
        private void Login(Users user)
        {
            MainWindow m = new MainWindow();
            ((MainWindowViewModel)m.DataContext).SetUser(user);
            m.Show();
            this.Close();
        }
        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (this.DataContext != null)
                ((dynamic)this.DataContext).EnteredPassword = ((PasswordBox)sender).Password;
        }

        private void TextBlock_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            RegistrationWindow r = new RegistrationWindow();
            r.Show();
        }
    }
}
