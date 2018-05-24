using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageService.Infrastructure
{
    public class Message
    {
        public string Args { get; set; }
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
