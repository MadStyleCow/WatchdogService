using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchdogService.Enums;

namespace WatchdogService.Classes
{
    public class BE_Message
    {
        /* Properties */
        public BE_MessageType Type { get; set; }
        public Object Content { get; set; }
        

        /* Constructors */
        public BE_Message() { }

        public BE_Message(BE_MessageType pType, Object pContent)
        {
            this.Content = pContent;
            this.Type = pType;
        }
    }
}
