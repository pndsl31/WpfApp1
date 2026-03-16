using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        public UserModel CurrentUser {  get; set; }

        public ICommand LoginCommand { get; set; }
        // public ICommand ForgotPasswordCommand { get; }

        public LoginViewModel()
        {
            CurrentUser = new UserModel();
            LoginCommand = new RelayCommand(ExecuteLogin);

            //ForgotPasswordCommand = new RelayCommand(ExecuteForgotPassword);


        }
        private void ExecuteLogin(object? parameter)
        {
            var password = parameter as PasswordBox;
            if (password != null)
            {
                CurrentUser.Password = password.Password;
            }
            if (CurrentUser.Username.Trim() == "admin" && CurrentUser.Password.Trim() == "1234")
            {

                var loginWindow = new Window1(CurrentUser);
                loginWindow.DataContext = new GradesWindowVM(CurrentUser);
                loginWindow.Show();
                Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);   
            }
        }
    }
}
