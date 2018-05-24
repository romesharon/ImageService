using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using ImageServiceGUI.Client;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using System.Windows;
using ImageService.Infrastructure;

namespace ImageServiceGUI.Model
{
    class ModelSettings : IModelSettings
    {
        string outputDirectory;
        string sourceName;
        string logName;
        int thumbnailSize;
        private bool connected;
        private ObservableCollection<string> handlers = new ObservableCollection<string>();
        public event PropertyChangedEventHandler PropertyChanged;

        public ModelSettings()
        {
            ClientSock client = ClientSock.Instance;
            client.MessageRecived += GetMessageFromServer;
        }

        public string OutputDirectory
        {
            get
            {
                return this.outputDirectory;
            }
            set
            {
                this.outputDirectory = value;
                this.NotifyPropertyChanged("OutputDirectory");
            }
        }
        public string SourceName
        {
            get
            {
                return this.sourceName;
            }
            set
            {
                this.sourceName = value;
                this.NotifyPropertyChanged("SourceName");
            }
        }
        public string LogName
        {
            get
            {
                return this.logName;
            }
            set
            {
                this.logName = value;
                this.NotifyPropertyChanged("LogName");
            }
        }
        public int ThumbnailSize
        {
            get
            {
                return this.thumbnailSize;
            }
            set
            {
                this.thumbnailSize = value;
                this.NotifyPropertyChanged("ThumbnailSize");
            }
        }
        public bool Connected
        {
            set
            {
                this.connected = value;
                this.NotifyPropertyChanged("Connected");
            }
            get { return this.connected; }
        }
        public ObservableCollection<string> Handlers
        {
            get { return this.handlers; }
        }
        private void NotifyPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }

        public void GetMessageFromServer(object sender, Message message)
        {
            switch (message.ID)
            {
                case CommandEnum.Settings:
                    try
                    {
                        Info info = JsonConvert.DeserializeObject<Info>(message.Args);
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            this.ThumbnailSize = info.ThumbnailSize;
                            this.OutputDirectory = info.OutputDir;
                            this.LogName = info.LogName;
                            this.SourceName = info.SourceName;
                            foreach (string handler in info.Handlers)
                            {
                                this.handlers.Add(handler);
                            }
                        }));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                case CommandEnum.CloseCommand:
                    try
                    {
                        string closedHandler = message.Args;
                        Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            this.handlers.Remove(closedHandler);
                        }));
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }
                    break;
                default:
                    Console.WriteLine("get different command");
                    break;
            }
        }

        public void GetInfoFromService()
        {
            ClientSock clientSock = ClientSock.Instance;
            Message m = new Message(CommandEnum.Settings, null);
            clientSock.Write(JsonConvert.SerializeObject(m));
        }

        public void RemoveHandler(string handlerPath)
        {
            Console.WriteLine("remove handler " + handlerPath);
            ClientSock client = ClientSock.Instance;
            Message info = new Message(CommandEnum.CloseCommand, handlerPath);
            string command = JsonConvert.SerializeObject(info);
            client.Write(command);
        }
    }
}
