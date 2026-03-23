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
            DataContext = new MainViewModel(_CurrentUser, this);
        }

      
    }
}
