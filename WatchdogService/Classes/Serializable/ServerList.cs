using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogService.Classes.Serializable
{
    public class ServerList
    {
        /* Properties */
        public List<Server> Servers { get; set; }

        /* Contructors */
        public ServerList() 
        {
            this.Servers = new List<Server>();
        }

        public ServerList(List<BE_Server> pServers)
        {
            this.Servers = new List<Server>();

            foreach(BE_Server pServer in pServers)
            {
                this.Servers.Add(new Server(pServer));
            }
        }
    }
}
