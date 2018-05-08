using ImageService.Logging;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    class Client : IClient
    {
        private List<TcpClient> clients;
        private ILoggingService loggingService;

        public Client(ILoggingService loggingService)
        {
            this.loggingService = loggingService;
            this.clients = new List<TcpClient>();
        }

        public void AddClient(TcpClient tcpClient)
        {
            clients.Add(tcpClient);
            Task thread = new Task(() =>
            {
                while(true)
                {
                    try
                    {
                        using (NetworkStream stream = tcpClient.GetStream())
                        using (StreamReader reader = new StreamReader(stream))
                        using (StreamWriter writer = new StreamWriter(stream))
                        {
                            //  get the command from the client
                            string commandLine = reader.ReadLine();
                            this.loggingService.Log(commandLine, MessageTypeEnum.INFO);
                        }
                    } catch(Exception e)
                    {
                        this.loggingService.Log(e.Message, MessageTypeEnum.FAIL);
                        break;
                    }
                }
                tcpClient.Close();
                this.clients.Remove(tcpClient);
            });
            thread.Start();
        }

            

        public void SendMessageToAllClients(MessageRecievedEventArgs args)
        {
            Task thread = new Task(() =>
            {
                foreach(TcpClient tcpClient in this.clients)
                {
                    using (NetworkStream stream = tcpClient.GetStream())
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        string message = string.Format("{0}${1}", args.Message, args.Status);
                        writer.WriteLine(message);
                    }
                }
            });
            thread.Start();
        }
    }
}
