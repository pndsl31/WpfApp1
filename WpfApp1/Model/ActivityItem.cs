using WpfApp1.ViewModel;

namespace WpfApp1.Model
{
    public class ActivityItem : ObservableObject
    {
        private int _activityId;
        private string _subjectName = string.Empty;
        private string _activityTitle = string.Empty;
        private DateTime _datePosted = DateTime.Now;
        private DateTime _deadline = DateTime.Now;
        private bool _isCompleted;

        public int ActivityId
        {
            get { return _activityId; }
            set
            {
                if (_activityId != value)
                {
                    _activityId = value;
                    OnPropertyCHanged(nameof(ActivityId));
                }
            }
        }

        public string SubjectName
        {
            get { return _subjectName; }
            set
            {
                if (_subjectName != value)
                {
                    _subjectName = value;
                    OnPropertyCHanged(nameof(SubjectName));
                }
            }
        }

        public string ActivityTitle
        {
            get { return _activityTitle; }
            set
            {
                if (_activityTitle != value)
                {
                    _activityTitle = value;
                    OnPropertyCHanged(nameof(ActivityTitle));
                }
            }
        }

        public DateTime DatePosted
        {
            get { return _datePosted; }
            set
            {
                if (_datePosted != value)
                {
                    _datePosted = value;
                    OnPropertyCHanged(nameof(DatePosted));
                }
            }
        }

        public DateTime Deadline
        {
            get { return _deadline; }
            set
            {
                if (_deadline != value)
                {
                    _deadline = value;
                    OnPropertyCHanged(nameof(Deadline));
                }
            }
        }

        public bool IsCompleted
        {
            get { return _isCompleted; }
            set
            {
                if (_isCompleted != value)
                {
                    _isCompleted = value;
                    OnPropertyCHanged(nameof(IsCompleted));
                }
            }
        }
    }
}
