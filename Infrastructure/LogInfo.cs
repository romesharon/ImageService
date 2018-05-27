using System;
using System.Collections.Generic;
using System.Text;
using ImageService.Logging.Modal;

namespace ImageService.Infrastructure
{
    /// <summary>
    /// Class LogInfo.
    /// </summary>
    public class LogInfo
    {
        /// <summary>
        /// Gets or sets the MSG.
        /// </summary>
        /// <value>The MSG.</value>
        public string Msg { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public MessageTypeEnum ID { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogInfo"/> class.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        /// <param name="id">The identifier.</param>
        public LogInfo(string msg, MessageTypeEnum id)
        {
            this.Msg = msg;
            this.ID = id;
        }
    }
}
