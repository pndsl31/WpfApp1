using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class MainDashboardViewModel : ObservableObject
    {
        public MainViewModel Navigation { get; set; }
        public UserModel CurrentUser { get; set; }

        public int TotalSubjects { get; set; }
        public string AverageGrade { get; set; }
        public string WelcomeMessage { get; set; }

        public MainDashboardViewModel(UserModel user, Window window)
        {
            CurrentUser = user;
            Navigation = new MainViewModel(user, window);

            TotalSubjects = 5;
            AverageGrade = "98.0";

            WelcomeMessage = $"Welcome to the Main Dashboard, {CurrentUser.Username}";
        }
    }

}
