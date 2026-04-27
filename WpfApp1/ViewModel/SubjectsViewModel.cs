using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.Data.SqlClient;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class SubjectsViewModel : ObservableObject
    {
        private static readonly string _conn = @"Data Source=(localdb)\MSSQLLocalDB;Database=poodle;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=""SQL Server Management Studio"";Command Timeout=0";

        public MainViewModel Navigation { get; set; }
        public UserModel CurrentUser { get; set; }
        public string WelcomeMessage { get; set; }

        public ObservableCollection<SubjectItem> SubjectList { get; set; }
        public ObservableCollection<ActivityItem> ActivityList { get; set; }

        public ActivityItem NewActivity { get; set; }

        private ActivityItem? _selectedActivity;
        public ActivityItem? SelectedActivity
        {
            get => _selectedActivity;
            set
            {
                _selectedActivity = value;
                OnPropertyCHanged(nameof(SelectedActivity));
            }
        }

        public ICommand AddActivityCommand { get; set; }
        public ICommand ToggleCompleteCommand { get; set; }
        public ICommand DeleteActivityCommand { get; set; }

        public SubjectsViewModel(UserModel currentUser, Window currentWindow)
        {
            Navigation = new MainViewModel(currentUser, currentWindow);
            CurrentUser = currentUser;
            WelcomeMessage = $"Welcome to the Subjects Dashboard, {currentUser?.Username ?? "Guest"}";

            SubjectList = new ObservableCollection<SubjectItem>
            {
                new SubjectItem { Name = "Database",                  Schedule = "MWF 8:00-9:00 AM"   },
                new SubjectItem { Name = "Networking",                Schedule = "TTh 9:00-10:30 AM"  },
                new SubjectItem { Name = "Event Driven Programming",  Schedule = "MWF 1:00-2:00 PM"   },
                new SubjectItem { Name = "History",                   Schedule = "TTh 1:00-2:30 PM"   },
                new SubjectItem { Name = "Integrative Programming",   Schedule = "Fri 3:00-5:00 PM"   },
            };

            ActivityList = new ObservableCollection<ActivityItem>();
            NewActivity = new ActivityItem();

            AddActivityCommand = new AsyncRelayCommand(ExecuteAddActivity);
            ToggleCompleteCommand = new AsyncRelayCommand(ExecuteToggleComplete);
            DeleteActivityCommand = new AsyncRelayCommand(ExecuteDeleteActivity);

            _ = LoadActivitiesAsync();
        }

        // ─── Load ────────────────────────────────────────────────────────────

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
                                SubjectName = reader["SubjectName"]?.ToString() ?? "",
                                ActivityTitle = reader["ActivityTitle"]?.ToString() ?? "",
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
                MessageBox.Show("Failed to load activities: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ─── Add ─────────────────────────────────────────────────────────────

        private async Task ExecuteAddActivity(object? par)
        {
            // Input validation
            if (string.IsNullOrWhiteSpace(NewActivity.SubjectName))
            {
                MessageBox.Show("Please enter a Subject Name.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(NewActivity.ActivityTitle))
            {
                MessageBox.Show("Please enter an Activity Title.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (NewActivity.Deadline < DateTime.Today)
            {
                MessageBox.Show("Deadline cannot be in the past.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var activity = new ActivityItem
            {
                SubjectName = NewActivity.SubjectName,
                ActivityTitle = NewActivity.ActivityTitle,
                DatePosted = DateTime.Now,
                Deadline = NewActivity.Deadline,
                IsCompleted = false
            };

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();

                    string query = @"
                        INSERT INTO Activities (SubjectName, ActivityTitle, DatePosted, Deadline, IsCompleted)
                        OUTPUT INSERTED.ActivityId
                        VALUES (@SubjectName, @ActivityTitle, @DatePosted, @Deadline, @IsCompleted)";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@SubjectName", activity.SubjectName);
                        cmd.Parameters.AddWithValue("@ActivityTitle", activity.ActivityTitle);
                        cmd.Parameters.AddWithValue("@DatePosted", activity.DatePosted);
                        cmd.Parameters.AddWithValue("@Deadline", activity.Deadline);
                        cmd.Parameters.AddWithValue("@IsCompleted", activity.IsCompleted);

                        var newId = await cmd.ExecuteScalarAsync();
                        activity.ActivityId = Convert.ToInt32(newId);
                    }
                }

                ActivityList.Add(activity);

                // Clear input fields
                NewActivity.SubjectName = "";
                NewActivity.ActivityTitle = "";
                NewActivity.Deadline = DateTime.Now;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to save activity: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ─── Toggle Done ──────────────────────────────────────────────────────

        private async Task ExecuteToggleComplete(object? par)
        {
            if (SelectedActivity == null)
            {
                MessageBox.Show("Please select an activity first.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

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
                MessageBox.Show("Failed to update activity: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ─── Delete ───────────────────────────────────────────────────────────

        private async Task ExecuteDeleteActivity(object? par)
        {
            if (SelectedActivity == null)
            {
                MessageBox.Show("Please select an activity to delete.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Delete '{SelectedActivity.ActivityTitle}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM Activities WHERE ActivityId = @ActivityId";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@ActivityId", SelectedActivity.ActivityId);
                        int rows = await cmd.ExecuteNonQueryAsync();

                        
                        if (rows > 0)
                        {
                            ActivityList.Remove(SelectedActivity);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete activity: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}