using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogService.Classes.Serializable
{
    public class Server
    {
        /* Properties */
        public string Identifier { get; set; }
        public bool Online { get; set; }
        public List<Player> Players { get; set; }

        /* Constructors */
        public Server() 
        {
            this.Players = new List<Player>();
        }

        public Server(BE_Server pServer)
        {
            this.Identifier = pServer.ServerIdentifier;
            this.Online = pServer.Online;
            this.Players = new List<Player>();
            
            foreach(BE_Player player in pServer.Players)
            {
                this.Players.Add(new Player(player));
            }
        }
    }
}
