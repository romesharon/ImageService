using ImageService.Controller;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
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
        private IImageController imageController;

        public Client(ILoggingService loggingService, IImageController imageController)
        {
            this.loggingService = loggingService;
            this.imageController = imageController;
            this.clients = new List<TcpClient>();
        }

        public void AddClient(TcpClient tcpClient)
        {
            clients.Add(tcpClient);
            Task thread = new Task(() =>
            {
                NetworkStream stream = tcpClient.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                while (true)
                {
                    try
                    {                

                        //  get the command from the client
                        string messageText = reader.ReadString();
                        this.loggingService.Log("recived: " + messageText, MessageTypeEnum.INFO);
                        Message clientMessage = JsonConvert.DeserializeObject<Message>(messageText);
                        if (clientMessage.ID == CommandEnum.Settings)
                        {
                            Info settings = GetSettingsFromConfig();
                            string settingsText = JsonConvert.SerializeObject(settings);
                            Message message = new Message(CommandEnum.Settings, settingsText);
                            this.loggingService.Log("settings text: " + settingsText, MessageTypeEnum.INFO);
                            writer.Write(JsonConvert.SerializeObject(message));
                        }
                        else if(clientMessage.ID == CommandEnum.Log)
                        {
                            LogList logList = GetLogList();
                            string logListText = JsonConvert.SerializeObject(logList);
                            Message message = new Message(CommandEnum.Log, logListText);
                            this.loggingService.Log("log text: " + logListText, MessageTypeEnum.INFO);
                            writer.Write(JsonConvert.SerializeObject(message));
                        }

                    }
                    catch (Exception e)
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

        private LogList GetLogList()
        {
            ObservableCollection<LogInfo> list = new ObservableCollection<LogInfo>
            {
                new LogInfo("hello world", MessageTypeEnum.INFO),
                new LogInfo("second log :)", MessageTypeEnum.INFO),
                new LogInfo("warning log :)", MessageTypeEnum.WARNING),
                new LogInfo("fail log :)", MessageTypeEnum.FAIL)


            };
            return new LogList(list);
        }

        private Info GetSettingsFromConfig()
        {
            string handlersText = ConfigurationManager.AppSettings.Get("Handler");
            List<string> handlers = new List<string>(handlersText.Split(';'));
            string outputDir = ConfigurationManager.AppSettings.Get("OutputDir");
            string sourceName = ConfigurationManager.AppSettings.Get("SourceName");
            string logName = ConfigurationManager.AppSettings.Get("LogName");
            int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
            return new Info(outputDir, sourceName, handlers, logName, thumbnailSize);
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
