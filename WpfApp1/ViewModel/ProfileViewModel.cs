using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class ProfileViewModel : ObservableObject
    {
        public UserModel CurrentUser { get; set; }

        public MainViewModel Navigation { get; set; }
        public string WelcomeMessage { get; set; }
        public string StudentId { get; set; }
        public string Course { get; set; }
        public string YearSection { get; set; }
        public string Email { get; set; }

        public ProfileViewModel(UserModel currentUser, Window currentWindow)
        {
            Navigation = new MainViewModel(currentUser, currentWindow);
            CurrentUser = currentUser;
            WelcomeMessage = $"Welcome to the Profile Dashboard, {currentUser.Username}";
            StudentId = "24-0150C";
            Course = "BS Information Technology";
            YearSection = "BSIT 2 - B";
            Email = $"{currentUser.Username}@sgen.edu.ph";
        }
    }
}
