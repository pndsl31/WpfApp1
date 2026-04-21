using Microsoft.Data.SqlClient;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class SubjectsViewModel : ObservableObject
    {
        private static readonly string _conn = @"Data Source=(localdb)\MSSQLLocalDB;
            Database=poodle;Integrated Security=True;Persist Security Info=False;
            Pooling=False;MultipleActiveResultSets=False;Encrypt=True;
            TrustServerCertificate=False;Application Name=""SQL Server Management Studio"";
            Command Timeout=0";

        public MainViewModel Navigation { get; set; }
        public UserModel CurrentUser { get; set; }
        public string WelcomeMessage { get; set; }
        public ObservableCollection<SubjectItem> SubjectList { get; set; }
        public ObservableCollection<ActivityItem> ActivityList { get; set; }

        private ActivityItem _newActivity = new ActivityItem();
        public ActivityItem NewActivity
        {
            get => _newActivity;
            set { _newActivity = value; OnPropertyCHanged(nameof(NewActivity)); }
        }

        private ActivityItem? _selectedActivity;
        public ActivityItem? SelectedActivity
        {
            get => _selectedActivity;
            set { _selectedActivity = value; OnPropertyCHanged(nameof(SelectedActivity)); }
        }

        public ICommand AddActivityCommand { get; set; }
        public ICommand DeleteActivityCommand { get; set; }
        public ICommand ToggleCompleteCommand { get; set; }

        public SubjectsViewModel(UserModel currentUser, Window currentWindow)
        {
            Navigation = new MainViewModel(currentUser, currentWindow);
            CurrentUser = currentUser;
            WelcomeMessage = $"Welcome to the Subjects Dashboard, {currentUser?.Username ?? "Guest"}";

            SubjectList = new ObservableCollection<SubjectItem>
            {
                new SubjectItem { Name = "Database", Schedule = "MWF 8:00-9:00 AM" },
                new SubjectItem { Name = "Networking", Schedule = "TTh 9:00-10:30 AM" },
                new SubjectItem { Name = "Event Driven Programming", Schedule = "MWF 1:00-2:00 PM" },
                new SubjectItem { Name = "History", Schedule = "TTh 1:00-2:30 PM" },
                new SubjectItem { Name = "Integrative Programming", Schedule = "Fri 3:00-5:00 PM" },
            };

            ActivityList = new ObservableCollection<ActivityItem>();

            AddActivityCommand = new AsyncRelayCommand(ExecuteAddActivity);
            DeleteActivityCommand = new AsyncRelayCommand(ExecuteDeleteActivity);
            ToggleCompleteCommand = new AsyncRelayCommand(ExecuteToggleComplete);

            _ = LoadActivitiesAsync();
        }

        private async Task LoadActivitiesAsync()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM Activities ORDER BY Deadline ASC";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            ActivityList.Add(new ActivityItem
                            {
                                ActivityId = Convert.ToInt32(reader["ActivityId"]),
                                SubjectName = reader["SubjectName"]?.ToString() ?? string.Empty,
                                ActivityTitle = reader["ActivityTitle"]?.ToString() ?? string.Empty,
                                DatePosted = Convert.ToDateTime(reader["DatePosted"]),
                                Deadline = Convert.ToDateTime(reader["Deadline"]),
                                IsCompleted = Convert.ToBoolean(reader["IsCompleted"])
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load activities: " + ex.Message);
            }
        }

        private async Task ExecuteAddActivity(object? par)
        {
            if (string.IsNullOrWhiteSpace(NewActivity.SubjectName) ||
                string.IsNullOrWhiteSpace(NewActivity.ActivityTitle))
            {
                MessageBox.Show("Please fill in the subject and activity title.");
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = @"INSERT INTO Activities (SubjectName, ActivityTitle, DatePosted, Deadline, IsCompleted)
                                     OUTPUT INSERTED.ActivityId
                                     VALUES (@SubjectName, @ActivityTitle, @DatePosted, @Deadline, @IsCompleted)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@SubjectName", NewActivity.SubjectName);
                        cmd.Parameters.AddWithValue("@ActivityTitle", NewActivity.ActivityTitle);
                        cmd.Parameters.AddWithValue("@DatePosted", DateTime.Now);
                        cmd.Parameters.AddWithValue("@Deadline", NewActivity.Deadline);
                        cmd.Parameters.AddWithValue("@IsCompleted", false);

                        int newId = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                        ActivityList.Add(new ActivityItem
                        {
                            ActivityId = newId,
                            SubjectName = NewActivity.SubjectName,
                            ActivityTitle = NewActivity.ActivityTitle,
                            DatePosted = DateTime.Now,
                            Deadline = NewActivity.Deadline,
                            IsCompleted = false
                        });
                    }
                }

                NewActivity = new ActivityItem();
                MessageBox.Show("Activity added!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add activity: " + ex.Message);
            }
        }

        private async Task ExecuteDeleteActivity(object? par)
        {
            if (SelectedActivity == null) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM Activities WHERE ActivityId = @ActivityId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ActivityId", SelectedActivity.ActivityId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                ActivityList.Remove(SelectedActivity);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete activity: " + ex.Message);
            }
        }

        private async Task ExecuteToggleComplete(object? par)
        {
            if (SelectedActivity == null) return;

            bool newStatus = !SelectedActivity.IsCompleted;

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = "UPDATE Activities SET IsCompleted = @IsCompleted WHERE ActivityId = @ActivityId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@IsCompleted", newStatus);
                        cmd.Parameters.AddWithValue("@ActivityId", SelectedActivity.ActivityId);
                        await cmd.ExecuteNonQueryAsync();
                    }
                }

                SelectedActivity.IsCompleted = newStatus;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update activity: " + ex.Message);
            }
        }
    }
}