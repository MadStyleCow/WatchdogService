using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogService.Classes
{
    [Serializable]
    public class WatchdogConfiguration
    {
        /* Properties */
        public int Port { get; set; }
        public List<BE_ServerConfiguration> Servers { get; set; }

        public WatchdogConfiguration() { }
    }
}
