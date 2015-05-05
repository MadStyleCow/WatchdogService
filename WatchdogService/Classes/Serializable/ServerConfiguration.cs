using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogService.Classes
{
    [Serializable]
    public class ServerConfiguration
    {
        /* Properties */
        public string Name { get; set; }
        public string Hostname { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }

        /* Constructors */
        public ServerConfiguration() { }
    }
}
