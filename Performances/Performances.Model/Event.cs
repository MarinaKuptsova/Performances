using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Performances.Model
{
    public class Event
    {
        public Guid Id { get; set; }
        public string Place { get; set; }
        public int ParticipantCount { get; set; }
        public string Description { get; set; }
        public Guid Photo { get; set; }
        public byte[] PhotoBytes { get; set; }
        public DateTime DateAndTime { get; set; }
    }
}
