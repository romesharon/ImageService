using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Commands
{
    public class NewFileCommand : ICommand
    {
        private IImageServiceModal m_modal;

        public NewFileCommand(IImageServiceModal modal)
        {
            m_modal = modal;            // Storing the Modal
        }

        /// <summary>
        /// execute the new file command
        /// </summary>
        /// <param name="args">path of file</param>
        /// <param name="result">success or fail</param>
        /// <returns></returns>
        public string Execute(string[] args, out bool result)
        {
            return this.m_modal.AddFile(args[0], out result);
        }
    }
}
