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
using System.ServiceModel;
using WatchdogService.Classes.Services;
using System.ServiceModel.Description;

namespace WatchdogService
{
    public partial class BattlEyeService : ServiceBase
    {
        /* Properties */
        WatchdogConfiguration Configuration { get; set; }
        List<BE_Server> ActiveServers { get; set; }
        ServiceHost Host { get; set; }

        /* Constructors */
        public BattlEyeService()
        {
            InitializeComponent();
        }

        /* Event handlers */
        protected override void OnStart(string[] args)
        {
            /* Initialize server list */
            ActiveServers = new List<BE_Server>();

            /* Read configuration set-up */
            Configuration = new WatchdogConfiguration()
            {
                Port = 8083,
                Servers = new List<BE_ServerConfiguration>()
            {
                new BE_ServerConfiguration()
                { Hostname = "193.19.118.182", Port = 2302, Name = "A2", Password = "banan" }
            }
            };

            // For each configuration (server) create a separate battleye listener.
            foreach (BE_ServerConfiguration ServerConfiguration in Configuration.Servers)
            {
                ActiveServers.Add(new BE_Server(new BattlEyeLoginCredentials()
                {
                    Host = IPAddress.Parse(ServerConfiguration.Hostname),
                    Password = ServerConfiguration.Password,
                    Port = ServerConfiguration.Port
                }));
            }

            /* Initialize the web-service */
            Host = new ServiceHost(typeof(GetPlayersService), new Uri(String.Format("http://localhost:{0}/{1}", Configuration.Port, GetPlayersService.URL)));
            // Enable metadata publishing.
            ServiceMetadataBehavior SMB = new ServiceMetadataBehavior();
            SMB.HttpGetEnabled = true;
            SMB.MetadataExporter.PolicyVersion = PolicyVersion.Policy15;
            Host.Description.Behaviors.Add(SMB);

            Host.Open();
        }

        protected override void OnStop()
        {
            Host.Close();

            // Shutdown all active server connections.
            foreach(BE_Server Server in ActiveServers)
            {
                Server.Shutdown();
            }
        }
    }
}
