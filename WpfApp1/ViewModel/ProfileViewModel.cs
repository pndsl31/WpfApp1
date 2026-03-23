using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class ProfileViewModel : ObservableObject
    {
        public UserModel CurrentUser { get; set; }

        public ProfileViewModel(UserModel user)
        {
            CurrentUser = user;
        }

    }

}
