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
using System.Threading;
using System.Threading.Tasks;


namespace ImageService.Server
{
    class Client : IClient
    {
        private List<TcpClient> clients;
        private ILoggingService loggingService;
        private IImageController imageController;
        private static Mutex mutex = new Mutex();

        /// <summary>
        /// Initializes a new instance of the <see cref="Client"/> class.
        /// </summary>
        /// <param name="loggingService">The logging service.</param>
        /// <param name="imageController">The image controller.</param>
        public Client(ILoggingService loggingService, IImageController imageController)
        {
            Console.WriteLine("In client constructor");
            this.loggingService = loggingService;
            this.loggingService.MessageRecieved += SendLogToAllClients;
            this.imageController = imageController;
            this.clients = new List<TcpClient>();
        }

        /// <summary>
        /// Adds the client.
        /// </summary>
        /// <param name="tcpClient">The TCP client.</param>
        public void AddClient(TcpClient tcpClient)
        {
            clients.Add(tcpClient); // add the client to the list
            Task thread = new Task(() =>
            {
                NetworkStream stream = tcpClient.GetStream();
                BinaryReader reader = new BinaryReader(stream);
                BinaryWriter writer = new BinaryWriter(stream);
                while (true)
                {
                    try
                    {
                        // read the message
                        string messageText = reader.ReadString();
                        this.loggingService.Log("recived: " + messageText, MessageTypeEnum.INFO);
                        Message clientMessage = JsonConvert.DeserializeObject<Message>(messageText);
                        Message message;
                        switch (clientMessage.ID)
                        {
                            case CommandEnum.Settings:
                                // send the settings
                                Info settings = GetSettingsFromConfig();
                                string settingsText = JsonConvert.SerializeObject(settings);
                                message = new Message(CommandEnum.Settings, settingsText);
                                mutex.WaitOne();
                                writer.Write(JsonConvert.SerializeObject(message));
                                mutex.ReleaseMutex();
                                break;
                            case CommandEnum.Log:
                                // send the logs
                                LogList logList = GetLogList();
                                string logListText = JsonConvert.SerializeObject(logList);
                                message = new Message(CommandEnum.Log, logListText);
                                mutex.WaitOne();
                                writer.Write(JsonConvert.SerializeObject(message));
                                mutex.ReleaseMutex();
                                break;
                            case CommandEnum.CloseCommand:
                                // close handler
                                string[] args = { clientMessage.Args };
                                this.loggingService.Log("client args for close commad: " + clientMessage.Args, MessageTypeEnum.INFO);
                                bool result;
                                string val = this.imageController.ExecuteCommand((int)clientMessage.ID, args, out result);
                                Message answer;
                                // faild to close handler
                                if (result == false)
                                {
                                    answer = new Message(CommandEnum.FailCommand, val);
                                }
                                else
                                {
                                    answer = new Message(CommandEnum.CloseCommand, val);
                                }
                                string answerText = JsonConvert.SerializeObject(answer);
                                this.loggingService.Log("answer from close commad " + answerText, MessageTypeEnum.INFO);
                                this.SendMessageToAllClients(answerText);
                                break;
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

        /// <summary>
        /// Gets the log list.
        /// </summary>
        /// <returns>LogList.</returns>
        private LogList GetLogList()
        {
            ObservableCollection<LogInfo> list = new ObservableCollection<LogInfo>();
            List<LogInfo> historyLogs = this.loggingService.HistoryLogs;
            foreach (LogInfo log in historyLogs)
            {
                list.Add(log);
            }
            return new LogList(list);
        }

        /// <summary>
        /// Gets the settings from configuration.
        /// </summary>
        /// <returns>Info.</returns>
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

        /// <summary>
        /// Sends the log to all clients.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="MessageRecievedEventArgs"/> instance containing the event data.</param>
        public void SendLogToAllClients(object sender, MessageRecievedEventArgs args)
        {
            LogInfo log = new LogInfo(args.Message, args.Status);
            ObservableCollection<LogInfo> list = new ObservableCollection<LogInfo> { log };
            LogList logList = new LogList(list);
            string messageData = JsonConvert.SerializeObject(logList);
            Message message = new Message(CommandEnum.Log, messageData);
            string toSend = JsonConvert.SerializeObject(message);

            Task thread = new Task(() =>
            {
                foreach(TcpClient tcpClient in this.clients)
                {
                    mutex.WaitOne();
                    (new BinaryWriter(tcpClient.GetStream())).Write(toSend);
                    mutex.ReleaseMutex();
                }
            });
            thread.Start();
        }

        /// <summary>
        /// Sends the message to all clients.
        /// </summary>
        /// <param name="message">The message.</param>
        public void SendMessageToAllClients(string message)
        { 
            Task thread = new Task(() =>
            {
                foreach (TcpClient tcpClient in this.clients)
                {
                    mutex.WaitOne();
                    (new BinaryWriter(tcpClient.GetStream())).Write(message);
                    mutex.ReleaseMutex();
                }
            });
            thread.Start();
        }


    }
}
