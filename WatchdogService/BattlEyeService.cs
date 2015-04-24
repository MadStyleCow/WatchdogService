using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using BattleNET;
using WatchdogService.Classes;
using System.Net;

namespace WatchdogService
{
    public partial class BattlEyeService : ServiceBase
    {
        List<BE_Server> Servers;

        public BattlEyeService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            // Initialize server list
            Servers = new List<BE_Server>();

            // Read configuration list from somewhere.
            List<BE_ServerConfiguration> Configurations = new List<BE_ServerConfiguration>() { new BE_ServerConfiguration() { 
                                                         Name = "A3", 
                                                         Hostname = "wogames.info", 
                                                         Port=2312,
                                                         Password="!Password1" }};
            

            // For each configuration (server) create a separate battleye listener.
            foreach(BE_ServerConfiguration config in Configurations)
            {
                Servers.Add(new BE_Server(new BattlEyeLoginCredentials()
                {
                    Host = IPAddress.Parse(config.Hostname),
                    Password = config.Password,
                    Port = config.Port
                }));
            }
        }

        protected override void OnStop()
        {
            // Shutdown all active server connections.
            foreach(BE_Server Server in Servers)
            {
                Server.Shutdown();
            }
        }
    }
}
