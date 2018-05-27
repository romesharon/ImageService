using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Logging.Modal
{
    /// <summary>
    /// Class MessageRecievedEventArgs.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class MessageRecievedEventArgs : EventArgs
    {
        /// <summary>
        /// The type
        /// </summary>
        private MessageTypeEnum type;

        /// <summary>
        /// Initializes a new instance of the <see cref="MessageRecievedEventArgs"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="message">The message.</param>
        public MessageRecievedEventArgs(MessageTypeEnum type, string message)
        {
            this.type = type;
            Message = message;
        }

        /// <summary>
        /// Gets or sets the status.
        /// </summary>
        /// <value>The status.</value>
        public MessageTypeEnum Status { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }
}
