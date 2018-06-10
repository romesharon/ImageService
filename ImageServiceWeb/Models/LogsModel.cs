using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using WebApplication2.Client;

namespace WebApplication2.Models
{
    public class LogsModel
    {
        //public delegate void NotifyChange();
        //public event NotifyChange Notify;
        internal ClientSock Client { get; set; }
        public List<LogInfo> LogsList_ { get; set; }
        public event EventHandler Update;

        public LogsModel()
        {
            Client = ClientSock.Instance;
            Client.MessageRecived += MessageRecivedHandler;
            this.LogsList_ = new List<LogInfo>();
            Message m = new Message(CommandEnum.Log, null);
            Client.Write(JsonConvert.SerializeObject(m));
        }
        
        private void MessageRecivedHandler(object sender, Message message)
        {
            if (message.ID == CommandEnum.Log)
            {
                try
                {
                    LogList logs = JsonConvert.DeserializeObject<LogList>(message.Args);
                    foreach (LogInfo l in logs.LogsList)
                    {
                        //if (!this.LogsList.Contains(l))
                        //{
                        //    this.LogsList.Add(l);
                        //}
                        this.LogsList_.Add(l);
                    }
                    Update?.Invoke(this, null);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}