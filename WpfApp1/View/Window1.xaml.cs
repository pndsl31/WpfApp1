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
using WpfApp1.ViewModel;

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
            DataContext = new MainDashboardViewModel(CurrentUser, this);
        }
    }
}
