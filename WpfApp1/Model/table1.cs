using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp1.ViewModel;
using System.Runtime.CompilerServices;
using WpfApp1.Model;

namespace WpfApp1.Model
{
    internal class table1 : ObservableObject
    {
        private string _ID = string.Empty;
        private string _subject = string.Empty;
        private string _grades = string.Empty;
        private DateTime _dateReported = DateTime.Now;
        private bool _isComplete = true;

        public string ID
        {
            get { return _ID; }
            set
            {
                if (_ID != value)
                {
                    _ID = value;
                    OnPropertyCHanged(nameof(ID));
                }
            }
        }

        public string Subject
        {
            get { return _subject; }
            set
            {
                if (_subject != value)
                {
                    _subject = value;
                    OnPropertyCHanged(nameof(Subject));
                }
            }
        }

        public string Grades
        {
            get { return _grades; }
            set
            {
                if (_grades != value)
                {
                    _grades = value;
                    OnPropertyCHanged(nameof(Grades));
                }
            }
        }

        public DateTime DateReported
        {
            get { return _dateReported; }
            set
            {
                if (_dateReported != value)
                {
                    _dateReported = value;
                    OnPropertyCHanged(nameof(DateReported));
                }
            }
        }

        public bool IsComplete
        {
            get { return _isComplete; }
            set
            {
                if (_isComplete != value)
                {
                    _isComplete = value;
                    OnPropertyCHanged(nameof(IsComplete));
                }
            }
        }
        private int _units = 3;
        public int Units
        {
            get { return _units; }
            set
            {
                if (_units != value)
                {
                    _units = value;
                    OnPropertyCHanged(nameof(Units));
                }
            }
        }
    }
}