using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using Performances.Client.Model;
using Performances.Model;

namespace Performances.Client.ViewModel
{
    public class MainViewModel : BaseModel
    {
        private MainWindow _window;
        public User MainUser { get; set; }
        public ScreenTypes CurrentScreenType { get; set; }

        public EventsViewModel eventsVM { get; set; }
        public UserLoginViewModel userLoginVM { get; set; }
        public RegistrationViewModel registrationVM { get; set; }

        public MainViewModel()
        {
            MainUser = new User();
            CurrentScreenType = ScreenTypes.Register;
            SetScreen();
        }

        public MainWindow Window
        {
            get
            {
                if (_window == null)
                {
                    _window = new MainWindow() { DataContext = this };
                }
                return _window;
            }
        }

        private object _currentContent;
        public object CurrentContent
        {
            get { return _currentContent; }
            set
            {
                if (_currentContent != value)
                {
                    _currentContent = value;
                    RaisePropertyChanged(nameof(CurrentContent));
                }
            }
        }
        
        public void SetScreen()
        {
            IScreen screen;
            switch (CurrentScreenType)
            {
                case ScreenTypes.Login:
                    screen = userLoginVM ?? (userLoginVM = new UserLoginViewModel(this));
                    break;
                case ScreenTypes.Register:
                    screen = registrationVM ?? (registrationVM = new RegistrationViewModel(this));
                    break;
                case ScreenTypes.Events:
                    screen = eventsVM ?? (eventsVM = new EventsViewModel(this));
                    break;
                    //TODO: Add Profile, EventProfile
                default:
                    screen = null;
                    break;
            }
            CurrentContent = screen.View();
            screen.Initialize();
        }
    }
}
