using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        #endregion

        #region Properties
        // The event that notifies about a new Command being recieved
        public event EventHandler<CommandRecievedEventArgs> CommandRecieved;         
        #endregion

        public ImageServer(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;


            string str = ConfigurationManager.AppSettings.Get("Handler");
            string[] directories = str.Split(';');

            foreach (string dir in directories)
            {
                this.m_logging.Log(string.Format("directory to listen {0}", dir), Logging.Modal.MessageTypeEnum.INFO);
                CreateHandler(dir);
            }
        }

        public void CreateHandler(string directory)
        {
            //  creating the handler
            IDirectoryHandler directoryHandler = new DirectoyHandler(this.m_controller, this.m_logging);
            //  adding the handler to the 'event'
            this.CommandRecieved += directoryHandler.OnCommandRecieved;
            //  set the closing handler function
            directoryHandler.DirectoryClose += CloseServer;
            //  start the handler for this directory
            directoryHandler.StartHandleDirectory(directory);
        }

        public void SendCommand(CommandRecievedEventArgs e)
        {
            //  sending the command to all handlers
            this.CommandRecieved?.Invoke(this, e);
        }

        public void CloseServer(object source, DirectoryCloseEventArgs e)
        {
            if (source is IDirectoryHandler directoryHandler)
            {
                this.CommandRecieved -= directoryHandler.OnCommandRecieved;
                directoryHandler.DirectoryClose -= CloseServer; //maybe a mistake..
            }
        }
    }
}
