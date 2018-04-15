using ImageService.Commands;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Controller
{
    public class ImageController : IImageController
    {
        private IImageServiceModal m_modal;                      // The Modal Object
        private Dictionary<int, ICommand> commands;

        public ImageController(IImageServiceModal modal)
        {
            m_modal = modal;                    // Storing the Modal Of The System
            commands = new Dictionary<int, ICommand>()
            {
                // For Now will contain NEW_FILE_COMMAND
                {(int)CommandEnum.NewFileCommand, new NewFileCommand(m_modal)}
            };
        }

        /// <summary>
        /// execute the command
        /// </summary>
        /// <param name="commandID">command id in the dictionary</param>
        /// <param name="args">arguments to the command</param>
        /// <param name="resultSuccesful">result</param>
        /// <returns></returns>
        public string ExecuteCommand(int commandID, string[] args, out bool resultSuccesful)
        {
            // Write Code Here
            if (this.commands.ContainsKey(commandID))
            {
                return this.commands[commandID].Execute(args, out resultSuccesful);
            }
            resultSuccesful = false;
            return $"Error: no command id {commandID}"; 
        }
    }
}
