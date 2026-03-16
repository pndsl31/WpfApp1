using System.Windows;
using WpfApp1.Model;

namespace WpfApp1.View
{
    public partial class ProfileWindow : Window
    {
        public UserModel CurrentUser { get; set; }
        public ProfileWindow(UserModel _CurrentUser)
        {
            CurrentUser = _CurrentUser;
            InitializeComponent();
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            View.Window1 dashboard = new View.Window1(CurrentUser);
            dashboard.Show();
            this.Close();
        }


        private void SubjectsWindow_Click(object sender, RoutedEventArgs e)
        {
            View.SubjectsWindow subjectsWindow = new View.SubjectsWindow(CurrentUser);
            subjectsWindow.Show();
            this.Close();
        }


        private void GradesWindow_Click(object sender, RoutedEventArgs e)
        {
            GradesWindow gradesWindow = new GradesWindow(CurrentUser);
            gradesWindow.Show();
            this.Close();
        }


        private void ProfileWindow_Click(object sender, RoutedEventArgs e)
        {
            View.ProfileWindow profileWindow = new View.ProfileWindow(CurrentUser);
            profileWindow.Show();
            this.Close();
        }

        private void LogoutWindow_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}