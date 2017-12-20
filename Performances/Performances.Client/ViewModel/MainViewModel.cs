using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Performances.Client.Model;

namespace Performances.Client.ViewModel
{
    public class MainViewModel : BaseModel
    {
        private MainWindow _window;
        public ScreenTypes CurrentScreenType { get; set; }

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

        public void SetScreen()
        {
            IScreen screen;
        }
    }
}
