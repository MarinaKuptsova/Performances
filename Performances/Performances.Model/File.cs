using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performances.Model
{
    public class File
    {
        public Guid Id { get; set; }
        public byte[] Bytes { get; set; }
        public string FileExtension { get; set; }
        public string Filename { get; set; }
    }
}
