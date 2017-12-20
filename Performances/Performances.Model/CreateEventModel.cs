using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Performances.Model
{
    public class CreateEventModel
    {
        public string Place { get; set; }
        public int ParticipantCount { get; set; }
        public string Description { get; set; }
        public CreativeTeam CreativeTeam { get; set; }
        public File FilePhoto { get; set; }
        public BitmapImage Photo { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
