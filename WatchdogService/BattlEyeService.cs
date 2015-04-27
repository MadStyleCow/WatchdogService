using BattleNET;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Web;
using System.ServiceProcess;
using WatchdogService.Classes;
using WatchdogService.Interfaces;
using WatchdogService.Services;

namespace WatchdogService
{
    public partial class BattlEyeService : ServiceBase
    {
        /* System properties */
        public static readonly BattlEyeService Instance = new BattlEyeService();
        string ConfigLocation = @"C:\WatchdogService\Config.xml";

        /* Properties */
        private WatchdogConfiguration Configuration { get; set; }
        public List<BE_Server> ActiveServers { get; set; }
        WebServiceHost wsHost { get; set; }

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
            Configuration = (WatchdogConfiguration) 
                Utilities.XMLSerializer.DeserializeFromFile(ConfigLocation, typeof(WatchdogConfiguration));

            // For each configuration (server) create a separate battleye listener.
            foreach (BE_ServerConfiguration ServerConfiguration in Configuration.Servers)
            {
                ActiveServers.Add(new BE_Server(new BattlEyeLoginCredentials()
                {
                    Host = IPAddress.Parse(ServerConfiguration.Hostname),
                    Password = ServerConfiguration.Password,
                    Port = ServerConfiguration.Port
                }, ServerConfiguration.Name));
            }

            /* Initialize the web-service */
            wsHost = new WebServiceHost(typeof(GetPlayers),
                new Uri(String.Format("http://localhost:{0}", Configuration.Port)));

            ServiceEndpoint wsEndpoint = wsHost.AddServiceEndpoint(typeof(IGetPlayers),
                new WebHttpBinding(), "");


            ServiceMetadataBehavior wsMetaBehaviour = new ServiceMetadataBehavior();
            wsMetaBehaviour.HttpGetEnabled = true;
            wsHost.Description.Behaviors.Add(wsMetaBehaviour);

            ServiceDebugBehavior wsDebugBehaviour = wsHost.Description.Behaviors.Find<ServiceDebugBehavior>();
            wsDebugBehaviour.HttpHelpPageEnabled = false;

            wsHost.Open();            
        }

        protected override void OnStop()
        {
            wsHost.Close();

            // Shutdown all active server connections.
            foreach (BE_Server Server in ActiveServers)
            {
                Server.Shutdown();
            }
        }
    }
}
