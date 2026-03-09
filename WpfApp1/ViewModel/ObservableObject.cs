using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace WpfApp1.ViewModel
{
    public class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        
        protected void OnPropertyCHanged([CallerMemberName] string? Name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Name ?? String.Empty));
        }
    }
}
