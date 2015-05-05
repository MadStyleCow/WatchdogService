using BattleNET;
using System;
using System.Threading;
using WatchdogService.Enums;

namespace WatchdogService.Classes
{
    class BE_Executor
    {
        /* Static variables */
        int Timeout = 5000;

        /* Properties */
        BattleNET.BattlEyeClient BEClient { get; set; }

        public delegate void MessageReceivedEventHandler(BE_Message pMessage);
        public event MessageReceivedEventHandler ClientMessageReceived;

        /* Constructors */
        public BE_Executor() { }

        /* Public methods */
        public void Listen(BattlEyeLoginCredentials pCredentials)
        {
            BEClient = new BattleNET.BattlEyeClient(pCredentials)
            {
                ReconnectOnPacketLoss = true               
            };

            BEClient.BattlEyeMessageReceived += new BattlEyeMessageEventHandler(MessageReceived);
            BEClient.BattlEyeDisconnected += new BattlEyeDisconnectEventHandler(Disconnected);
            BEClient.BattlEyeConnected += new BattlEyeConnectEventHandler(Connected);

            while (BEClient.Connect() != BattlEyeConnectionResult.Success)
            {
                Thread.Sleep(Timeout);
                Console.WriteLine("Retrying RCON connection to {0}:{1}", pCredentials.Host, pCredentials.Port);
            }
        }

        public void Stop()
        {
            if (BEClient.Connected)
            {
                BEClient.Disconnect();
            }
        }

        public void Execute(BattleNET.BattlEyeCommand pCommand, string pParameters = "")
        {
            if (BEClient.Connected)
            {
                if (BEClient.SendCommand(pCommand, pParameters) == 256)
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        public void Execute(String pCommand)
        {
            if (BEClient.Connected)
            {
                if (BEClient.SendCommand(pCommand) == 256)
                {
                    throw new ArgumentException();
                }
            }
            else
            {
                throw new InvalidOperationException();
            }
        }

        /* Private methods */
        private void Disconnected(BattlEyeDisconnectEventArgs pArgs)
        {
            SendMessage(new BE_Message(BE_MessageType.ServerDisconnected, null));

            if(pArgs.DisconnectionType != BattlEyeDisconnectionType.Manual)
            {
                while (BEClient.Connect() != BattlEyeConnectionResult.Success)
                {
                    Console.WriteLine("Attempting to reconnect to RCON server");
                    Thread.Sleep(Timeout);
                }
            }
        }

        private void MessageReceived(BattlEyeMessageEventArgs pArgs)
        {
            Console.WriteLine(pArgs.Message);
            SendMessage(BE_Parser.Parse(pArgs.Message));
        }

        private void Connected(BattlEyeConnectEventArgs pArgs)
        {
            if (pArgs.ConnectionResult == BattlEyeConnectionResult.Success)
            {
                SendMessage(new BE_Message(BE_MessageType.ServerConnected, null));
                Execute(BattlEyeCommand.Players);
            }
        }

        private void SendMessage(BE_Message pMessage)
        {
            if (ClientMessageReceived != null)
            {
                ClientMessageReceived(pMessage);
            }
        }
    }
}
