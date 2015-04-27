using System;
using WatchdogService.Interfaces;

namespace WatchdogService.Classes.Services
{
    public class GetPlayersService : IGetPlayersService
    {
        /* Properties */
        public static string URL = "getplayers";

        /* Constructors */
        public string GetPlayers()
        {
            return "test_string" + DateTime.Now.ToString();
        }
    }
}
