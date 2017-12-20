using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Performances.Client.Model;
using Performances.Model;

namespace Performances.Client.ViewModel
{
    public class EventsViewModel : BaseModel, IScreen
    {
        public object CurrentDialog { get; set; }
        public MainViewModel Parent { get; set; }
        public ObservableCollection<Event> AllEvents { get; set; }
        public BitmapImage Photo { get; set; }

        public EventsViewModel(MainViewModel mvm)
        {
            Parent = mvm;
        }
        public void Initialize()
        {
            //var result = DataAccess.DataAccess.GetAllEvents();
            //AllEvents = new ObservableCollection<Event>(result);
        }

        public object View()
        {
            throw new NotImplementedException();
        }
    }
}
