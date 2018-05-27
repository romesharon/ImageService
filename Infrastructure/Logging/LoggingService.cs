
using ImageService.Infrastructure;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    public class LoggingService : ILoggingService
    {
        public event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        private List<LogInfo> historyLogs = new List<LogInfo>();

        public List<LogInfo> HistoryLogs { get => historyLogs; set => historyLogs = value; }

        /// <summary>
        /// write message to log
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="type">enum</param>
        public void Log(string message, MessageTypeEnum type)
        {
            HistoryLogs.Add(new LogInfo(message, type));
            MessageRecieved?.Invoke(this, new MessageRecievedEventArgs(type, message));
        }
    }
}
