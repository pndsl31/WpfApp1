using System.Windows;
using WpfApp1.Model;
using WpfApp1.ViewModel;

public class ProfileViewModel
{
    public MainViewModel Navigation { get; }
    public UserModel CurrentUser { get; }


    public string Username { get; }
    public string StudentId { get; }
    public string Course { get; }
    public string YearSection { get; }
    public string Email { get; }
    public string WelcomeMessage { get; }

    public ProfileViewModel(UserModel currentUser, Window currentWindow)
    {
        Navigation = new MainViewModel(currentUser, currentWindow);
        CurrentUser = currentUser;

        Username = currentUser.Username;
        StudentId = currentUser.StudentNumber;
        Course = currentUser.Course;
        YearSection = currentUser.YearSection;
        Email = currentUser.Email;
        WelcomeMessage = $"Welcome to the Profile Dashboard, {currentUser.Username}";
    }
}