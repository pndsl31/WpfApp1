using System.Windows;

namespace WpfApp1.View
{
    public partial class GradesWindow : Window
    {
        public GradesWindow()
        {
            InitializeComponent();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            View.GradesWindow gradesWindow = new View.GradesWindow();
            gradesWindow.Show();
            this.Close();
        }

        private void SubjectsWindow_Click(object sender, RoutedEventArgs e)
        {
            View.SubjectsWindow subjectsWindow = new View.SubjectsWindow();
            subjectsWindow.Show();
            this.Close();
        }

        private void GradesWindow_Click(object sender, RoutedEventArgs e)
        {
            View.GradesWindow gradesWindow = new View.GradesWindow();
            gradesWindow.Show();
            this.Close();
        }

        private void ProfileWindow_Click(object sender, RoutedEventArgs e)
        {
            View.ProfileWindow profileWindow = new View.ProfileWindow();
            profileWindow.Show();
            this.Close();
        }

        private void LogoutWindow_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            View.MainWindow mainWindow = new View.MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}