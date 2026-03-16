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

        public table1 newAccount { get; set; }

        public GradesWindowVM(UserModel CurrentUser)
        {
            _CurrentUser = CurrentUser;
            table1List = new ObservableCollection<table1>()
            {
                new table1 { ID = " 01", Subject = "Math", Grades = "100", DateReported = new DateTime(2024, 5, 10), IsComplete = true },
                new table1 { ID = " 02", Subject = "Science", Grades = "99", DateReported = new DateTime(2024, 5, 12), IsComplete = true },
                new table1 { ID = " 03", Subject = "English", Grades = "98", DateReported = new DateTime(2024, 5, 15), IsComplete = true },
                new table1 { ID = " 04", Subject = "History", Grades = "97", DateReported = new DateTime(2024, 5, 18), IsComplete = true },
                new table1 { ID = " 05", Subject = "MAPEH", Grades = "96", DateReported = new DateTime(2024, 5, 20), IsComplete = true }
            };
            newAccount = new table1();

            SaveCommand = new RelayCommand(ExecuteSaveCommand);
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


    }
}
