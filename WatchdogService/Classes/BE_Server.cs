using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WatchdogService.Enums;
using BattleNET;

namespace WatchdogService.Classes
{
    public class BE_Server
    {
        /* Properties */
        BE_Executor Executor { get; set; }
        Dictionary<int, BE_Player> Players { get; set; }

        /* Constructors */
        public BE_Server(BattlEyeLoginCredentials pCredentials)
        {
            this.Executor = new BE_Executor(pCredentials);
            this.Players = new Dictionary<int, BE_Player>();
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
            if (!Players.ContainsKey(pPayload.Id))
            {
                Players.Add(pPayload.Id, pPayload);
            }
        }

        private void RemovePlayer(BE_Player pPayload)
        {
            if (Players.ContainsKey(pPayload.Id))
            {
                Players.Remove(pPayload.Id);
            }
        }

        private void UpdatePlayer(BE_Player pPayload)
        {
            if (Players.ContainsKey(pPayload.Id))
            {
                if (Players[pPayload.Id].Guid != null | Players[pPayload.Id].Guid != pPayload.Guid)
                {
                    Players[pPayload.Id].Guid = pPayload.Guid;
                }
            }
        }
    }
}
