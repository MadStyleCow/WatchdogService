using WatchdogService.Classes.Serializable;
using WatchdogService.Interfaces;

namespace WatchdogService.Services
{
    public class GetPlayers : IGetPlayers
    {
        /* Service implementation */
        public ServerList GetPlayerList()
        {
            ServerList resultSet = new ServerList(BattlEyeService.Instance.ActiveServers);
            return resultSet;
        }
    }
}
