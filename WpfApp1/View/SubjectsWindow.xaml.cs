using System.Windows;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class SubjectsWindow : Window
    {
        public UserModel CurrentUser { get; set; }
        public SubjectsWindow(UserModel _CurrentUser)
        {
            InitializeComponent();
            CurrentUser = _CurrentUser;
            DataContext = new SubjectsViewModel(_CurrentUser, this);
        }


    }
}