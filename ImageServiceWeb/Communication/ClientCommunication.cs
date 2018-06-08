using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

//singeltone!
namespace WebApplication2.Communication
{
    public class ClientCommunication : IClientCommunication
    {
        private TcpClient client;
        private bool isStopped;
        public delegate void UpdateResponseArrived(CommandRecievedEventArgs responseObj);
        public event UpdateResponseArrived UpdateResponse;
        private static ClientCommunication instance;
        private static Mutex mutex = new Mutex();
        private static Mutex readMutex = new Mutex();
        public bool IsConnected { get; set; }

       
        private ClientCommunication()
        {
            this.IsConnected = this.Start();
        }

      
        public static ClientCommunication Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ClientCommunication();
                }
                return instance;
            }
        }

        /// <summary>
        /// Start function.
        /// starts the tcp connection.
        /// </summary>
        /// <returns></returns>
        private bool Start()
        {
            try
            {
                IPEndPoint ep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345);
                client = new TcpClient();
                client.Connect(ep);
                Console.WriteLine("Connection established");
                isStopped = false;
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
        }

        
        public void Send(CommandRecievedEventArgs commandRecievedEventArgs)
        {
      
            try
            {
                string jsonCommand = JsonConvert.SerializeObject(commandRecievedEventArgs);
                NetworkStream stream = client.GetStream();
                BinaryWriter writer = new BinaryWriter(stream);
                // Send data to server
                Console.WriteLine($"Send {jsonCommand} to Server");
                mutex.WaitOne();
                writer.Write(jsonCommand);
                mutex.ReleaseMutex();
            }
            catch (Exception ex)
            {

            }
         
        }

        /// <summary>
        /// RecieveCommand function.
        /// creates task and reads new messages.
        /// </summary>
        public void Recieve()
        {
            new Task(() =>
            {
                try
                {
                    while (!isStopped)
                    {
                        NetworkStream stream = client.GetStream();
                        BinaryReader reader = new BinaryReader(stream);
                        readMutex.WaitOne();
                        string response = reader.ReadString();
                        readMutex.ReleaseMutex();
                        Console.WriteLine($"Recieve {response} from Server");
                        CommandRecievedEventArgs responseObj = JsonConvert.DeserializeObject<CommandRecievedEventArgs>(response);
                        this.UpdateResponse?.Invoke(responseObj);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }).Start();
        }

        public void Close()
        {
            CommandRecievedEventArgs commandRecievedEventArgs = new CommandRecievedEventArgs((int)CommandEnum.CloseClient, null, "");
            Send(commandRecievedEventArgs);
            isStopped = true;
            client.Close();
        }
    }
}