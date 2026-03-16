using System;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Model;
using WpfApp1.ViewModel;



namespace WpfApp1.View
{
    public partial class GradesWindow : Window
    {
        public UserModel CurrentUser { get; set; }
        public GradesWindow(UserModel _CurrentUser)
        {
            CurrentUser = _CurrentUser;
            InitializeComponent();
        }

      

        private void DataGrid_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SubjectsWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void GradesWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ProfileWindow_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LogoutWindow_Dashboard_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
