using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    class HomePageViewModel
    {
        // var newViewModel = new HomePageViewModel(CurrentUser);
        public UserModel _CurrentUser { get; set; }
        public HomePageViewModel(UserModel CurrentUser)
        {
            _CurrentUser = CurrentUser;
        }
        
    }
}
