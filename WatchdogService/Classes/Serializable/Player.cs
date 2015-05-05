using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchdogService.Classes.Serializable
{
    public class Player
    {
        /* Properties */
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public String Guid { get; set; }
        public String IP { get; set; }

        /* Constructors */
        public Player() { }

        public Player(BE_Player pPlayer)
        {
            this.Id = pPlayer.Id;
            this.Name = pPlayer.Name;
            this.Guid = pPlayer.Guid;
            this.IP = pPlayer.IP;
        }
    }
}
