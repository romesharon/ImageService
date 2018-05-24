using System;
using System.Collections.Generic;
using System.Text;
using ImageService.Logging.Modal;

namespace ImageService.Infrastructure
{
    public class LogInfo
    {
        public string Msg { get; set; }
        public MessageTypeEnum ID { get; set; }

        public LogInfo(string msg, MessageTypeEnum id)
        {
            this.Msg = msg;
            this.ID = id;
        }
    }
}
