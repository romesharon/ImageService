using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace WebApplication2.Client
{
    class ClientSock
    {
        private const string END_COMMUNICATION = "close";
        private NetworkStream stream;
        private BinaryReader reader;
        private BinaryWriter writer;
        private static ClientSock instance;
        private TcpClient client;
        public event EventHandler<Message> MessageRecived;
        private bool isConnected = false;
        private ClientSock()
        {
            try
            {
                // try open connection with the server
                IPEndPoint ec = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                this.client = new TcpClient();
                this.client.Connect(ec);
                this.stream = this.client.GetStream();
                this.reader = new BinaryReader(stream);
                this.writer = new BinaryWriter(stream);
                Read(); // read from server the old logs and settings
                IsConnected = true;
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                IsConnected = false;
            }
        }

        /// <summary>
        /// Gets the instance.
        /// </summary>
        /// <value>The instance.</value>
        public static ClientSock Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientSock();
                }
                return instance;
            }
        }

        public bool IsConnected
        {
            get
            {
                return isConnected;
            }
            set
            {
                isConnected = value;
            }
        }

        /// <summary>
        /// Writes the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        public void Write(string message)
        {
            try
            {
                this.writer.Write(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        /// <summary>
        /// Reads this instance.
        /// </summary>
        public void Read()
        {
            Task readThread = new Task(() =>
            {
                string message;

                while(true)
                {
                    try
                    {
                        message = this.reader.ReadString();
                        Message serverMessage = JsonConvert.DeserializeObject<Message>(message);
                        if (serverMessage.ID == CommandEnum.CloseServer)
                        {
                            StopCommunication();
                            break;
                        }
                        Console.WriteLine(message);
                        this.MessageRecived?.Invoke(this, serverMessage);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        break;
                    }
                }
                
            });
            readThread.Start();

        }

        /// <summary>
        /// Stops the communication.
        /// </summary>
        private void StopCommunication()
        {
            Console.WriteLine("end communication with the server");
            // maybe should change the order
            this.client.Close();
            this.stream.Close();
            this.reader.Close();
            this.writer.Close();
        }
    }
}
