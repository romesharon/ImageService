using ImageService.Infrastructure;
using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging
{
    /// <summary>
    /// Interface ILoggingService
    /// </summary>
    public interface ILoggingService
    {
        /// <summary>
        /// Gets or sets the history logs.
        /// </summary>
        /// <value>The history logs.</value>
        List<LogInfo> HistoryLogs { get; set; }

        event EventHandler<MessageRecievedEventArgs> MessageRecieved;
        /// <summary>
        /// Logs the specified message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        void Log(string message, MessageTypeEnum type);
    }
}
