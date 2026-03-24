using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Model;
using System.Collections.ObjectModel;

namespace WpfApp1.ViewModel
{
    public class SubjectsViewModel : ObservableObject
    {
        public MainViewModel Navigation { get; set; }
        public UserModel CurrentUser { get; set; }
        public string WelcomeMessage { get; set; }
        public ObservableCollection<SubjectItem> SubjectList { get; set; }
        public SubjectsViewModel(UserModel currentUser, Window currentWindow)
        {
            Navigation = new MainViewModel(currentUser, currentWindow);
            CurrentUser = currentUser;
            WelcomeMessage = $"Welcome to the Subjects Dashboard, {currentUser?.Username ?? "Guest"}";

            SubjectList = new ObservableCollection<SubjectItem>
            {
                new SubjectItem {Name = "Database", Schedule = "MWF 8:00-9:00 AM"},
                new SubjectItem {Name = "Networking", Schedule = "TTh 9:00-10:30 AM"},
                new SubjectItem {Name = "Event Driven Programming", Schedule = "MWF 1:00-2:00 PM"},
                new SubjectItem {Name = "History", Schedule = "TTh 1:00-2:30 PM"},
                new SubjectItem {Name = "Integrative Programming", Schedule = "Fri 3:00-5:00 PM"},
            };
        }
        //public class SubjectItem
        //{
        //    public string Name { get; set; } = string.Empty;
        //    public string Schedule { get; set; } = string.Empty;
        //}
    }
}
