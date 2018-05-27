using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using ImageService.Server;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class CloseCommand : ICommand
    {
        private ImageServer m_imageServer;

        /// <summary>
        /// Initializes a new instance of the <see cref="CloseCommand"/> class.
        /// </summary>
        /// <param name="m_imageServer">The m image server.</param>
        public CloseCommand(ImageServer m_imageServer)
        {
            this.m_imageServer = m_imageServer;
        }

        /// <summary>
        /// Executes the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <param name="result">if set to <c>true</c> [result].</param>
        /// <returns>System.String.</returns>
        public string Execute(string[] args, out bool result)
        {
            try
            {
                // closing the handler
                CommandRecievedEventArgs command = new CommandRecievedEventArgs((int)CommandEnum.CloseCommand, null, args[0]);
                //this.m_imageServer.SendCommand(command);


                // remove handler from app config
                string str = ConfigurationManager.AppSettings.Get("Handler");
                string[] directories = str.Split(';');
                string newHandlers = "";
                int counter = 0;
                foreach (string dir in directories)
                {
                    if (!dir.Equals(args[0]))
                    {
                        if (counter != 0)
                        {
                            newHandlers += ";";
                        }
                        newHandlers += dir;
                        counter++;
                    }
                }
                Console.Write("new handlers are: " + newHandlers);
                ConfigurationManager.AppSettings.Set("Handler", newHandlers);

                result = true;
                return args[0];
            }
            catch (Exception e)
            {
                result = false;
                return e.Message;
            }
        }
    }
}
