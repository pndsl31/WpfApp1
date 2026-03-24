using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    internal class GradesWindowVM : ObservableObject
    {
        // var newViewModel = new HomePageViewModel(CurrentUser);
        public ObservableCollection<table1> table1List { get; set; }
        public UserModel _CurrentUser { get; set; }

        public ICommand SaveCommand { get; set; }

        public ICommand DeleteCommand { get; set; }

        public ICommand ClearCommand { get; set; }
        public MainViewModel Navigation {  get; set; }

        public table1 newAccount { get; set; }
        public string WelcomeMessage { get; set; }

        public GradesWindowVM(UserModel currentUser, Window currentWindow)
        {
            Navigation = new MainViewModel(currentUser, currentWindow);
            _CurrentUser = currentUser;
            WelcomeMessage = $"Welcome to the Grades Dashboard, {currentUser.Username}";
            table1List = new ObservableCollection<table1>()
            {
                new table1 { ID = " 01", Subject = "Database", Grades = "100", DateReported = new DateTime(2024, 5, 10), IsComplete = true },
                new table1 { ID = " 02", Subject = "Networking", Grades = "99", DateReported = new DateTime(2024, 5, 12), IsComplete = true },
                new table1 { ID = " 03", Subject = "Event Driven Programming", Grades = "98", DateReported = new DateTime(2024, 5, 15), IsComplete = true },
                new table1 { ID = " 04", Subject = "History", Grades = "97", DateReported = new DateTime(2024, 5, 18), IsComplete = true },
                new table1 { ID = " 05", Subject = "Integrative Programming", Grades = "96", DateReported = new DateTime(2024, 5, 20), IsComplete = true }
            };
            newAccount = new table1();

            _selectedItem = new table1();
            SaveCommand = new RelayCommand(ExecuteSaveCommand);
            DeleteCommand = new RelayCommand(ExecuteDeleteCommand);
            ClearCommand = new RelayCommand(ExecuteClearCommand);

        }
        public void ExecuteSaveCommand(object? par)
        {
            table1 newGrade = new table1()
            {
                ID = newAccount.ID,
                Subject = newAccount.Subject,
                Grades = newAccount.Grades,
                DateReported = newAccount.DateReported,
                IsComplete = newAccount.IsComplete
            };

            table1List.Add(newGrade);
            MessageBox.Show("Success", "New Record Added", MessageBoxButton.OK, MessageBoxImage.Information);

            newAccount.ID = "";
            newAccount.Subject = "";
            newAccount.Grades = "";
            newAccount.DateReported = DateTime.Now;
            newAccount.IsComplete = true;
        }

        private table1 _selectedItem;
        
        public table1 SelectedItem
        {
            get { return _selectedItem; }
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

        public void ExecuteDeleteCommand(object? par)
        {
            table1List.Remove(SelectedItem);
        }

        public void ExecuteClearCommand(object? par)
        {
            newAccount.ID = string.Empty;
            newAccount.Subject = string.Empty;
            newAccount.Grades = string.Empty;

        }
    }
}
