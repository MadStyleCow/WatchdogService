using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WatchdogService.Enums;
using BattleNET;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace WatchdogService.Classes
{
    [Serializable]
    public class BE_Server
    {
        /* Properties */
        [JsonIgnore]
        BE_Executor Executor { get; set; }

        public String ServerIdentifier { get; set; }
        public List<BE_Player> Players { get; set; }

        /* Constructors */
        private BE_Server() { }

        public BE_Server(BattlEyeLoginCredentials pCredentials, String pIdentifier)
        {
            this.ServerIdentifier = pIdentifier;
            this.Executor = new BE_Executor(pCredentials);
            this.Players = new List<BE_Player>();
            this.Executor.ClientMessageReceived += new BE_Executor.MessageReceivedEventHandler(RouteMessage);
        }

        /* Public methods */
        public void Shutdown()
        {
            this.Executor.Stop();
        }

        /* Private methods */
        private void RouteMessage(BE_Message pMessage)
        {
            switch (pMessage.Type)
            {
                case BE_MessageType.Connected:
                    AddPlayer((BE_Player) pMessage.Content);
                    break;

                case BE_MessageType.Disconnected:
                    RemovePlayer((BE_Player)pMessage.Content);
                    break;

                case BE_MessageType.Unverified:
                    UpdatePlayer((BE_Player)pMessage.Content);
                    break;

                case BE_MessageType.Verified:
                    UpdatePlayer((BE_Player)pMessage.Content);
                    break;
        
                default:
                    break;
            }
        }

        /* Internal player tracking */
        private void AddPlayer(BE_Player pPayload)
        {
            if (!Players.Exists(p => p.Id == pPayload.Id))
            {
                Players.Add(pPayload);
            }
        }

        private void RemovePlayer(BE_Player pPayload)
        {
            if (Players.Exists(p => p.Id == pPayload.Id))
            {
                Players.Remove(Players.Find(p => p.Id == pPayload.Id));
            }
        }

        private void UpdatePlayer(BE_Player pPayload)
        {
            if (Players.Exists(p => p.Id == pPayload.Id))
            {
                var playerObject = Players.Find(p=>p.Id == pPayload.Id);
                if (playerObject.Guid != null | playerObject.Guid != pPayload.Guid)
                {
                    playerObject.Guid = pPayload.Guid;
                }
            }
        }
    }
}
