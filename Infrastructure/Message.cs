using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageService.Infrastructure
{
    /// <summary>
    /// Class Message.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Gets or sets the arguments.
        /// </summary>
        /// <value>The arguments.</value>
        public string Args { get; set; }
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public CommandEnum ID { get; set; }

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="id"></param>
        /// <param name="args"></param>
        public Message(CommandEnum id, string args)
        {
            this.Args = args;
            this.ID = id;
        }
    }
}
