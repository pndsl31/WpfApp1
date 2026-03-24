using System.Windows;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1.View
{
    public partial class ProfileWindow : Window
    {
        public UserModel CurrentUser { get; set; }
        public ProfileWindow(UserModel _CurrentUser)
        {
            CurrentUser = _CurrentUser;
            InitializeComponent();
            DataContext = new ProfileViewModel(_CurrentUser, this);
        }
    }
}