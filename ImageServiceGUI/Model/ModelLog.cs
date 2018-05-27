using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageServiceGUI.Client;
using Newtonsoft.Json;
using ImageService.Infrastructure.Enums;
using static System.Net.Mime.MediaTypeNames;
using System.Windows;

namespace ImageServiceGUI.Model
{
    public class ModelLog : IModelLog
    {
        private ObservableCollection<LogInfo> logsList = new ObservableCollection<LogInfo>();
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<LogInfo> LogsList
        {
            get
            {
                return this.logsList;
            }
            set
            {
                logsList = value;
                this.NotifyPropertyChanged("LogList");
            }
        }

        public ModelLog()
        {
            ClientSock client = ClientSock.Instance;
            client.MessageRecived += ReadLogsFromService;
        }

        private void NotifyPropertyChanged(string property)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }


        public void GetLogsFromService()
        {
            ClientSock clientSock = ClientSock.Instance;
            string msg = JsonConvert.SerializeObject(new Message(CommandEnum.Log, null));
            clientSock.Write(msg);
        }

        public void ReadLogsFromService (object sender, Message message)
        {
            // if the command is log command
            if (message.ID == CommandEnum.Log)
            {
                try
                {
                   
                    LogList logs = JsonConvert.DeserializeObject<LogList>(message.Args);
                    foreach (LogInfo log in logs.LogsList)
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(new Action(() =>
                        {
                            this.LogsList.Add(log);
                        }));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
