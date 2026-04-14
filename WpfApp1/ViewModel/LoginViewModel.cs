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
using Microsoft.Data.SqlClient;

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
        private async void ExecuteLogin(object? parameter)
        {
            var password = parameter as PasswordBox;
            if (password != null)
            {
                CurrentUser.Password = password.Password;
            }
            //if (CurrentUser.Username.Trim() == "admin" && CurrentUser.Password.Trim() == "1234")
            //{ 
            //    var loginWindow = new Window1(CurrentUser);
            //    //loginWindow.DataContext = new Window1(CurrentUser);
            //    loginWindow.Show();
            //    Application.Current.MainWindow.Close();
            //}
            //else
            //{
            //    MessageBox.Show("Invalid username or password", "Error", MessageBoxButton.OK, MessageBoxImage.Error);   
            //}

            string connectionString = @"Server=CCL2-20;Database=poodle;User Id=sa;Password=ccl2;TrustServerCertificate=True;";
            bool isLoginValid = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    // THE TRADITIONAL (AND DANGEROUS) WAY: String Concatenation
                    // We are directly pasting whatever the user typed into our database command.
                    string query = "SELECT * FROM Users WHERE Username = @username AND Password = @password";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", CurrentUser.Username);
                        command.Parameters.AddWithValue("@password", CurrentUser.Password);

                        await connection.OpenAsync();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            if (reader.HasRows)
                            {
                                isLoginValid = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection failed: " + ex.Message);
                return;
            }

            if (isLoginValid)
            {
                MessageBox.Show("Login Successful! Welcome.", "Success",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                var loginWindow = new Window1(CurrentUser);
                //loginWindow.DataContext = new Window1(CurrentUser);
                loginWindow.Show();
                Application.Current.MainWindow.Close();
            }
            else
            {
                MessageBox.Show("Invalid Username or Password.", "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
