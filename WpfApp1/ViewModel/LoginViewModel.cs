using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.View;
using Microsoft.Data.SqlClient;

namespace WpfApp1.ViewModel
{
    public class LoginViewModel : ObservableObject
    {
        public UserModel CurrentUser { get; set; }
        public ICommand LoginCommand { get; set; }

        public LoginViewModel()
        {
            CurrentUser = new UserModel();
            LoginCommand = new RelayCommand(ExecuteLogin);
        }

        private async void ExecuteLogin(object? parameter)
        {
            
            var passwordBox = parameter as PasswordBox;
            if (passwordBox != null)
            {
                CurrentUser.Password = passwordBox.Password;
            }

            
            if (string.IsNullOrWhiteSpace(CurrentUser.Username))
            {
                MessageBox.Show("Please enter your username.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(CurrentUser.Password))
            {
                MessageBox.Show("Please enter your password.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Database=poodle;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=""SQL Server Management Studio"";Command Timeout=0";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

        
                    string query = @"SELECT Username, Password, Email, 
                                            StudentNumber, Course, YearSection 
                                     FROM Users 
                                     WHERE Username = @username 
                                       AND Password = @password";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@username", CurrentUser.Username);
                        command.Parameters.AddWithValue("@password", CurrentUser.Password);

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
                        {
                            
                            if (await reader.ReadAsync())
                            {
                                
                                //reading each column and save
                                CurrentUser.Username = reader["Username"]?.ToString() ?? "";
                                CurrentUser.Email = reader["Email"]?.ToString() ?? "";
                                CurrentUser.StudentNumber = reader["StudentNumber"]?.ToString() ?? "";
                                CurrentUser.Course = reader["Course"]?.ToString() ?? "";
                                CurrentUser.YearSection = reader["YearSection"]?.ToString() ?? "";

                                
                                MessageBox.Show("Login Successful! Welcome.", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                                var dashboard = new Window1(CurrentUser);
                                dashboard.Show();
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
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database connection failed: " + ex.Message,
                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}