using System;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using Newtonsoft.Json;
using WebApplication2.Client;

namespace WebApplication2.Models
{
    public class ConfigModel
    {
        internal ClientSock Client { get; set; }
        public delegate void NotifyChange();
        public event NotifyChange Notify;
        public string SelectedHandler { get; set; }


        public ConfigModel()
        {
            try
            {
                Client = ClientSock.Instance;
                Client.MessageRecived += MessageRecivedHandler;

                OutputDir = "";
                SourceName = "";
                LogName = "";
                ThumbnailSize = 0;
                Handlers = new ObservableCollection<string>();
                Enabled = false;
                Message m = new Message(CommandEnum.Settings, null);
                Client.Write(JsonConvert.SerializeObject(m));
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void MessageRecivedHandler(object sender, Message message)
        {
            switch (message.ID)
            {
                case CommandEnum.Settings:
                    try
                    {
                        Info info = JsonConvert.DeserializeObject<Info>(message.Args);
                        this.ThumbnailSize = info.ThumbnailSize;
                        this.OutputDir = info.OutputDir;
                        this.LogName = info.LogName;
                        this.SourceName = info.SourceName;
                        foreach (string handler in info.Handlers)
                        {
                            Handlers.Add(handler);
                        }
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
                        Handlers.Remove(closedHandler);
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

       

        public void CloseHandler()
        {
            Message messageInfo = new Message(CommandEnum.CloseCommand, this.SelectedHandler);
            string message = JsonConvert.SerializeObject(messageInfo);
            Client.Write(message);
        }


        [Required]
        [DataType(DataType.Text)]
        public bool Enabled { get; set; }

        [Required]
        [Display(Name = "OutputDir")]
        public string OutputDir { get; set; }
        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "SourceName")]
        public string SourceName { get; set; }

        [Required]
        [DataType(DataType.Text)]
        [Display(Name = "LogName")]
        public string LogName { get; set; }

        [Required]
        [Display(Name = "ThumbnailSize")]
        public int ThumbnailSize { get; set; }

        [Required]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Handlers")]
        public ObservableCollection<string> Handlers { get; set; }
    }
}