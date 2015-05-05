using BattleNET;
using System;
using System.Collections.Generic;
using WatchdogService.Enums;

namespace WatchdogService.Classes
{
    [Serializable]
    public class BE_Server
    {
        /* Properties */
        public String ServerIdentifier { get; set; }
        public List<BE_Player> Players { get; set; }
        public bool Online { get; set; }

        /* Internal properties */
        BE_Executor Executor { get; set; }

        /* Constructors */
        private BE_Server() { }

        public BE_Server(BattlEyeLoginCredentials pCredentials, String pIdentifier)
        {
            this.ServerIdentifier = pIdentifier;
            this.Players = new List<BE_Player>();
            this.Online = false;
            this.Executor = new BE_Executor();
            this.Executor.ClientMessageReceived += new BE_Executor.MessageReceivedEventHandler(RouteMessage);
            this.Executor.Listen(pCredentials);
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

                case BE_MessageType.Players:
                    PlayerList((List<BE_Player>)pMessage.Content);
                    break;

                case BE_MessageType.ServerConnected:
                    Connected();
                    break;

                case BE_MessageType.ServerDisconnected:
                    Disconnected();
                    break;
        
                default:
                    break;
            }
        }
        /* Internal server state tracking */
        private void Connected()
        {        
            this.Online = true;
            this.Players.Clear();
        }

        private void Disconnected()
        {
            this.Online = false;
            this.Players.Clear();
        }

        /* Internal player tracking */
        private void PlayerList(List<BE_Player> pPayload)
        {     
            foreach(BE_Player player in pPayload)
            {
                Players.Add(player);
            }
        }

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
