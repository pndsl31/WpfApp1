using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1.Model
{
    public class UserModel : ObservableObject
    {
        private string _username = "";
        private string _password = "";
        private string _email = "";
        private string _studentNumber = "";
        private string _course = "";
        private string _yearSection = "";

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
        public string Email
        {
            get { return _email; }
            set
            {
                _email = value;
                OnPropertyCHanged(nameof(Email));
            }
        }

        public string StudentNumber
        {
            get { return _studentNumber; }
            set
            {
                _studentNumber = value;
                OnPropertyCHanged(nameof(StudentNumber));
            }
        }

        public string Course
        {
            get { return _course; }
            set
            {
                _course = value;
                OnPropertyCHanged(nameof(Course));
            }
        }
        public string YearSection
        {
            get { return _yearSection; }
            set
            {
                _yearSection = value;
                OnPropertyCHanged(nameof(YearSection));
            }
        }
    }
}
