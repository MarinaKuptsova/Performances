using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Performances.Client.Model;
using Performances.Model;

namespace Performances.Client.ViewModel
{
    public class MainViewModel : BaseModel
    {
        private MainWindow _window;
        public User MainUser { get; set; }
        public ScreenTypes CurrentScreenType { get; set; }

        public MainViewModel()
        {
            MainUser = new User();
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
        }
    }
}
