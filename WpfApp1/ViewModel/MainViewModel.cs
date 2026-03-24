using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp1.Model;
using WpfApp1.View;

namespace WpfApp1.ViewModel
{
    public class MainViewModel : ObservableObject
    {
        private Window _currentWindow;
        
        public UserModel CurrentUser { get; set; }
        public ICommand GoToGradesCommand { get; set; }
        public ICommand GoToSubjectsCommand { get; set; }
        public ICommand GoToProfileCommand { get; set; }
        public ICommand GoToMainDashboardCommand { get; set; }
        public ICommand LogoutCommand { get; set; }



        public MainViewModel(UserModel user, Window currentWindow)
        {
            CurrentUser = user;
            _currentWindow = currentWindow;
            GoToGradesCommand = new RelayCommand(GoToGrades);
            GoToSubjectsCommand = new RelayCommand(GoToSubjects);
            GoToProfileCommand = new RelayCommand(GoToProfile);
            GoToMainDashboardCommand = new RelayCommand(GoToMainDashboard);
            LogoutCommand = new RelayCommand(Logout);
        }

        public void GoToGrades(object? parameter)
        {
            var win = new GradesWindow(CurrentUser);
            //win.DataContext = new GradesWindowVM(CurrentUser);
            win.Show();

            _currentWindow.Close();
        }

        public void GoToSubjects(object? parameter)
        {
            var win = new SubjectsWindow(CurrentUser);
            //win.DataContext = new SubjectsViewModel(CurrentUser);
            win.Show();

            _currentWindow.Close();
        }

        public void GoToProfile(object? parameter)
        {
            var win = new ProfileWindow(CurrentUser);
            //win.DataContext = new ProfileViewModel(CurrentUser);
            win.Show();

            _currentWindow.Close();
        }

        public void GoToMainDashboard(object? parameter)
        {
            var win = new Window1(CurrentUser);
            //win.DataContext = new Window1(CurrentUser);
            win.Show();

            _currentWindow.Close();
        }

        public void Logout(object? parameter)
        {
            var win = new MainWindow();
            win.Show();

            _currentWindow.Close();
        }
    }
}
