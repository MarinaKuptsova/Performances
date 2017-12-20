using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Performances.Client.Model
{
    public interface IScreen
    {
        object View();
        void Initialize();
    }
}
