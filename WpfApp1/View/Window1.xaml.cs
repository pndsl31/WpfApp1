using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Model;

namespace WpfApp1.View
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public UserModel _CurrentUser { get; set; }
        public Window1(UserModel CurrentUser)
        {
            _CurrentUser = CurrentUser;
            InitializeComponent();
        }


        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LogoutWindow_Dashboard_Click(object sender, RoutedEventArgs e)
        {
            View.MainWindow mainWindow = new View.MainWindow();
            mainWindow.Show();
            this.Close();
        }

        private void ProfileWindow_Click(object sender, RoutedEventArgs e)
        {
            View.ProfileWindow profileWindow = new View.ProfileWindow(_CurrentUser);
            profileWindow.Show();
            this.Close();
        }

        private void GradesWindow_Click(object sender, RoutedEventArgs e)
        {
            View.GradesWindow gradesWindow = new View.GradesWindow(_CurrentUser);
            gradesWindow.DataContext = new ViewModel.GradesWindowVM(_CurrentUser) ;
            gradesWindow.Show();
            this.Close();
        }

        private void SubjectsWindow_Click(object sender, RoutedEventArgs e)
        {
            View.SubjectsWindow subjectsWindow = new View.SubjectsWindow(_CurrentUser);
            subjectsWindow.Show();
            this.Close();
        }
    }
}
