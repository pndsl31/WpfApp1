using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class SubjectsViewModel : ObservableObject
    {
        public UserModel CurrentUser { get; set; }

        public SubjectsViewModel(UserModel user)
        {
            CurrentUser = user;
        }
    }
}
