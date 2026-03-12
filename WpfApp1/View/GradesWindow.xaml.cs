using System;
using System.Windows;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class GradesWindow : Window
    {
        GradesWindowVM vm = new GradesWindowVM();

        public GradesWindow()
        {
            InitializeComponent();
            DataContext = vm;
        }

        private void AddGrade_Click(object sender, RoutedEventArgs e)
        {
            table1 newGrade = new table1()
            {
                ID = txtID.Text,
                Subject = txtSubject.Text,
                Grades = txtGrades.Text,
                DateReported = dpDate.SelectedDate ?? DateTime.Now,
                IsComplete = true
            };

            vm.table1List.Add(newGrade);

            txtID.Text = "";
            txtSubject.Text = "";
            txtGrades.Text = "";
            dpDate.SelectedDate = null;
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
