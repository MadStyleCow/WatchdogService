using BattleNET;
using System;
using System.Threading;

namespace WatchdogService.Classes
{
    class BE_Executor
    {
        /* Properties */
        BattleNET.BattlEyeClient BEClient { get; set; }

        public delegate void MessageReceivedEventHandler(BE_Message pMessage);
        public event MessageReceivedEventHandler ClientMessageReceived;

        /* Constructors */
        public BE_Executor(BattlEyeLoginCredentials pCredentials)
        {
            this.Listen(pCredentials);
        }

        /* Public methods */
        public void Listen(BattlEyeLoginCredentials pCredentials)
        {
            BEClient = new BattleNET.BattlEyeClient(pCredentials)
            {
                ReconnectOnPacketLoss = true               
            };

            while (BEClient.Connect() != BattlEyeConnectionResult.Success)
            {
                Thread.Sleep(1000);
                Console.WriteLine("Retrying RCON connection to {0}:{1}", pCredentials.Host, pCredentials.Port);
            }

            BEClient.BattlEyeMessageReceived += new BattlEyeMessageEventHandler(MessageReceived);
            BEClient.BattlEyeDisconnected += new BattlEyeDisconnectEventHandler(Disconnected);
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
            if(pArgs.DisconnectionType != BattlEyeDisconnectionType.Manual)
            {
                while (BEClient.Connect() != BattlEyeConnectionResult.Success)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private void MessageReceived(BattlEyeMessageEventArgs pArgs)
        {
            Console.WriteLine(pArgs.Message);

            if(ClientMessageReceived != null)
            {
                ClientMessageReceived(BE_Parser.Parse(pArgs.Message));
            }
        }
    }
}
