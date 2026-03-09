using System;
using WpfApp1.Model;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel;
using System.Runtime.CompilerServices;

namespace WpfApp1.Model
{
    public class UserModel : ObservableObject
    {
        private string _username = string.Empty;
        private string _password = string.Empty;

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username != value)
                {
                    _username = value;
                    OnPropertyCHanged(nameof(Username));
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    OnPropertyCHanged(nameof(Password));
                }
            }
        }
    }
}
