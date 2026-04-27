using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class MainDashboardViewModel : ObservableObject
    {
        private static readonly string _conn = @"Data Source=(localdb)\MSSQLLocalDB;
            Database=poodle;Integrated Security=True;Persist Security Info=False;
            Pooling=False;MultipleActiveResultSets=False;Encrypt=True;
            TrustServerCertificate=False;Application Name=""SQL Server Management Studio"";
            Command Timeout=0";

        public MainViewModel Navigation { get; set; }
        public UserModel CurrentUser { get; set; }
        public string WelcomeMessage { get; set; }

        private int _totalSubjects;
        public int TotalSubjects
        {
            get => _totalSubjects;
            set { _totalSubjects = value; OnPropertyCHanged(nameof(TotalSubjects)); }
        }

        private string _averageGrade = "N/A";
        public string AverageGrade
        {
            get => _averageGrade;
            set { _averageGrade = value; OnPropertyCHanged(nameof(AverageGrade)); }
        }

        private string _targetGrade = "0";
        public string TargetGrade
        {
            get => _targetGrade;
            set { _targetGrade = value; OnPropertyCHanged(nameof(TargetGrade)); }
        }

        private string _gradeStatusMessage = string.Empty;
        public string GradeStatusMessage
        {
            get => _gradeStatusMessage;
            set { _gradeStatusMessage = value; OnPropertyCHanged(nameof(GradeStatusMessage)); }
        }

        public ICommand SaveTargetGradeCommand { get; set; }

        public MainDashboardViewModel(UserModel user, Window window)
        {
            CurrentUser = user;
            Navigation = new MainViewModel(user, window);
            WelcomeMessage = $"Welcome to the Main Dashboard, {CurrentUser.Username}";
            SaveTargetGradeCommand = new AsyncRelayCommand(ExecuteSaveTargetGrade);
            _ = LoadDashboardDataAsync();
        }

        private async Task LoadDashboardDataAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();

                    // computation of the average taking the units to account as well
                    string avgQuery = "SELECT SUM(CAST(Grades AS FLOAT) * Units) / SUM(Units) FROM table1";
                    using (SqlCommand cmd = new SqlCommand(avgQuery, connection))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != DBNull.Value && result != null)
                        {
                            double avg = Convert.ToDouble(result);
                            AverageGrade = avg.ToString("F2");
                        }
                    }

                    // total subjects count
                    string countQuery = "SELECT COUNT(DISTINCT Subject) FROM table1";
                    using (SqlCommand cmd = new SqlCommand(countQuery, connection))
                    {
                        var result = await cmd.ExecuteScalarAsync();
                        TotalSubjects = Convert.ToInt32(result);
                    }

                    // target grade 
                    string targetQuery = "SELECT TargetGrade FROM UserSettings WHERE Username = @Username";
                    using (SqlCommand cmd = new SqlCommand(targetQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", CurrentUser.Username);
                        var result = await cmd.ExecuteScalarAsync();
                        if (result != DBNull.Value && result != null)
                        {
                            TargetGrade = result.ToString()!;
                        }
                    }
                }

                UpdateGradeStatus();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load dashboard data: " + ex.Message);
            }
        }

        private async Task ExecuteSaveTargetGrade(object? par)
        {
            if (!double.TryParse(TargetGrade, out double target))
            {
                MessageBox.Show("Please enter a valid number for your target grade.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();

                    
                    string query = @"
                        IF EXISTS (SELECT 1 FROM UserSettings WHERE Username = @Username)
                            UPDATE UserSettings SET TargetGrade = @TargetGrade WHERE Username = @Username
                        ELSE
                            INSERT INTO UserSettings (Username, TargetGrade) VALUES (@Username, @TargetGrade)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Username", CurrentUser.Username);
                        cmd.Parameters.AddWithValue("@TargetGrade", target);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                UpdateGradeStatus();
                MessageBox.Show("Target grade saved!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save target grade: " + ex.Message);
            }
        }

        private void UpdateGradeStatus()
        {
            if (double.TryParse(AverageGrade, out double avg) && double.TryParse(TargetGrade, out double target))
            {
                if (avg >= target)
                    GradeStatusMessage = $" You are meeting your target of {target}!";
                else
                    GradeStatusMessage = $" You need {(target - avg):F2} more points to hit your target of {target}.";
            }
            else
            {
                GradeStatusMessage = "Set a target grade to track your progress.";
            }
        }
    }
}