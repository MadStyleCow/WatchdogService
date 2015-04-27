using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatchdogService.Interfaces;
using Newtonsoft.Json;

namespace WatchdogService.Services
{
    public class GetPlayers : IGetPlayers
    {
        /* Service implementation */
        public string GetPlayerList()
        {
            var ActiveServers = BattlEyeService.Instance.ActiveServers;
            var json = JsonConvert.SerializeObject(ActiveServers, Formatting.Indented);
            return json;
        }
    }
}
