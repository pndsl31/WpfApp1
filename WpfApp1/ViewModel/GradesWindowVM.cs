using Microsoft.Data.SqlClient;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    internal class GradesWindowVM : ObservableObject
    {
        private static readonly string _conn = @"Data Source=(localdb)\MSSQLLocalDB;Database=poodle;Integrated Security=True;Persist Security Info=False;Pooling=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Application Name=""SQL Server Management Studio"";Command Timeout=0";

        public ObservableCollection<table1> table1List { get; set; }
        public UserModel _CurrentUser { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }
        public ICommand ClearCommand { get; set; }
        public ICommand UpdateCommand { get; set; }

        public MainViewModel Navigation { get; set; }
        public table1 newAccount { get; set; }
        public string WelcomeMessage { get; set; }

        public GradesWindowVM(UserModel currentUser, Window currentWindow)
        {
            Navigation = new MainViewModel(currentUser, currentWindow);
            _CurrentUser = currentUser;
            WelcomeMessage = $"Welcome to the Grades Dashboard, {currentUser.Username}";

            table1List = new ObservableCollection<table1>();
            newAccount = new table1();
            _selectedItem = new table1();

            SaveCommand = new AsyncRelayCommand(ExecuteSaveCommand);
            DeleteCommand = new AsyncRelayCommand(ExecuteDeleteCommand);
            ClearCommand = new RelayCommand(ExecuteClearCommand);
            UpdateCommand = new AsyncRelayCommand(ExecuteUpdateCommand);

            // FIX: use discard so the compiler doesn't warn about unawaited async
            _ = LoadItemsFromFile();
        }

        // ─── Save ─────────────────────────────────────────────────────────────

        public async Task ExecuteSaveCommand(object? par)
        {
            // ── Input validation ──────────────────────────────────────────────
            if (string.IsNullOrWhiteSpace(newAccount.ID))
            {
                MessageBox.Show("ID cannot be empty.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(newAccount.Subject))
            {
                MessageBox.Show("Subject cannot be empty.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(newAccount.Grades))
            {
                MessageBox.Show("Grades cannot be empty.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(newAccount.Grades, out double gradeValue) || gradeValue < 0 || gradeValue > 100)
            {
                MessageBox.Show("Grades must be a number between 0 and 100.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            // ─────────────────────────────────────────────────────────────────

            table1 newGrade = new table1
            {
                ID = newAccount.ID,
                Subject = newAccount.Subject,
                Grades = newAccount.Grades,
                DateReported = newAccount.DateReported,
                IsComplete = newAccount.IsComplete
            };

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = "INSERT INTO table1 (ID, Subject, Grades, DateReported, IsComplete) VALUES (@ID, @Subject, @Grades, @DateReported, @IsComplete)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", newGrade.ID);
                        command.Parameters.AddWithValue("@Subject", newGrade.Subject);
                        command.Parameters.AddWithValue("@Grades", newGrade.Grades);
                        command.Parameters.AddWithValue("@DateReported", newGrade.DateReported);
                        command.Parameters.AddWithValue("@IsComplete", newGrade.IsComplete);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            // Only add to UI list after DB confirms success
                            table1List.Add(newGrade);
                            MessageBox.Show("Record successfully added.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                            // Clear input fields
                            newAccount.ID = "";
                            newAccount.Subject = "";
                            newAccount.Grades = "";
                            newAccount.DateReported = DateTime.Now;
                            newAccount.IsComplete = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ─── Selected Item ────────────────────────────────────────────────────

        private table1 _selectedItem;
        public table1 SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                OnPropertyCHanged(nameof(SelectedItem));
                if (SelectedItem != null)
                {
                    newAccount.ID = SelectedItem.ID;
                    newAccount.Subject = SelectedItem.Subject;
                    newAccount.Grades = SelectedItem.Grades;
                    newAccount.DateReported = SelectedItem.DateReported;
                    newAccount.IsComplete = SelectedItem.IsComplete;
                }
            }
        }

        // ─── Delete ───────────────────────────────────────────────────────────

        private async Task ExecuteDeleteCommand(object? par)
        {
            if (SelectedItem == null || string.IsNullOrWhiteSpace(SelectedItem.ID))
            {
                MessageBox.Show("Please select a record to delete.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var confirm = MessageBox.Show(
                $"Delete record with ID '{SelectedItem.ID}'?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirm != MessageBoxResult.Yes) return;

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = "DELETE FROM table1 WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", SelectedItem.ID);
                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        // FIX: only remove from UI list if DB delete actually succeeded
                        if (rowsAffected > 0)
                        {
                            table1List.Remove(SelectedItem);
                            MessageBox.Show("Record deleted.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ─── Clear ────────────────────────────────────────────────────────────

        public void ExecuteClearCommand(object? par)
        {
            newAccount.ID = string.Empty;
            newAccount.Subject = string.Empty;
            newAccount.Grades = string.Empty;
            newAccount.DateReported = DateTime.Now;
            newAccount.IsComplete = true;
        }

        // ─── Update ───────────────────────────────────────────────────────────

        private async Task ExecuteUpdateCommand(object? par)
        {
            if (SelectedItem == null || string.IsNullOrWhiteSpace(_selectedItem.ID))
            {
                MessageBox.Show("Please select a record to update.", "No Selection",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(newAccount.Subject))
            {
                MessageBox.Show("Subject cannot be empty.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!double.TryParse(newAccount.Grades, out double gradeValue) || gradeValue < 0 || gradeValue > 100)
            {
                MessageBox.Show("Grades must be a number between 0 and 100.", "Validation Error",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = "UPDATE table1 SET Subject = @Subject, Grades = @Grades, DateReported = @DateReported, IsComplete = @IsComplete WHERE ID = @ID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ID", _selectedItem.ID);
                        command.Parameters.AddWithValue("@Subject", newAccount.Subject);
                        command.Parameters.AddWithValue("@Grades", newAccount.Grades);
                        command.Parameters.AddWithValue("@DateReported", newAccount.DateReported);
                        command.Parameters.AddWithValue("@IsComplete", newAccount.IsComplete);

                        int rowsAffected = await command.ExecuteNonQueryAsync();

                        if (rowsAffected > 0)
                        {
                            // Update in-memory object after DB confirms
                            _selectedItem.Subject = newAccount.Subject;
                            _selectedItem.Grades = newAccount.Grades;
                            _selectedItem.DateReported = newAccount.DateReported;
                            _selectedItem.IsComplete = newAccount.IsComplete;

                            MessageBox.Show("Record updated.", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Database error: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // ─── Load ─────────────────────────────────────────────────────────────

        private async Task LoadItemsFromFile()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(_conn))
                {
                    await connection.OpenAsync();
                    string query = "SELECT * FROM table1";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            table1List.Add(new table1
                            {
                                ID = reader["ID"]?.ToString() ?? "",
                                Subject = reader["Subject"]?.ToString() ?? "",
                                Grades = reader["Grades"]?.ToString() ?? "",
                                DateReported = Convert.ToDateTime(reader["DateReported"]?.ToString() ?? DateTime.Now.ToString()),
                                IsComplete = Convert.ToBoolean(reader["IsComplete"]?.ToString() ?? "false")
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load grades: " + ex.Message, "Error",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}