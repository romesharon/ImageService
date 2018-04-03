using ImageService.Modal;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Infrastructure;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Logging.Modal;
using System.Text.RegularExpressions;



namespace ImageService.Controller.Handlers
{
    public class DirectoyHandler : IDirectoryHandler
    {
        #region Members
        private IImageController m_controller;              // The Image Processing Controller
        private ILoggingService m_logging;
        private FileSystemWatcher m_dirWatcher;             // The Watcher of the Dir
        private string m_path;                              // The Path of directory
        public const int NUMBERS_OF_FILES = 4;
        private string[] types = { ".jpg", ".png", ".gif", ".bmp" };
        #endregion
        // The Event That Notifies that the Directory is being closed
        public event EventHandler<DirectoryCloseEventArgs> DirectoryClose;

        public DirectoyHandler(IImageController controller, ILoggingService logging)
        {
            this.m_controller = controller;
            this.m_logging = logging;
        }

        public void OnCommandRecieved(object sender, CommandRecievedEventArgs e)
        {
            //check if the command is close
            if (e.CommandID == (int)CommandEnum.CloseCommand)
            {
                this.m_logging.Log("Close command in Handler", MessageTypeEnum.INFO);
                CloseHandler();
                return;
            }

            string message = m_controller.ExecuteCommand(e.CommandID, e.Args, out bool result);

            //  execute seccessfull
            if (result == true)
            {
                this.m_logging.Log($"Success execute command id {e.CommandID}, " + message, MessageTypeEnum.INFO);
            }

            //  execute failed
            else
            {
                this.m_logging.Log($"Error: execute command id  {e.CommandID} failed, " + message, MessageTypeEnum.FAIL);
            }

        }

        private void CloseHandler()
        {
            this.m_dirWatcher.Created -= new FileSystemEventHandler(DirectoryCreated);
            this.m_dirWatcher.Changed -= new FileSystemEventHandler(DirectoryCreated);
            this.m_dirWatcher.EnableRaisingEvents = false;
            m_dirWatcher.Dispose();

            DirectoryCloseEventArgs directoryCloseEvent = new DirectoryCloseEventArgs(this.m_path, $"Directory {this.m_path} has been closed");
            DirectoryClose?.Invoke(this, directoryCloseEvent);
        }

        private void DirectoryCreated(object source, FileSystemEventArgs e)
        {
            string[] args = { e.FullPath };
            //  check if the file is from the type we look
            if (types.Contains(Path.GetExtension(e.FullPath).ToLower()))
            {
                //  new file is enterd
                CommandRecievedEventArgs eventArgs = new CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, args, this.m_path);
                this.OnCommandRecieved(this, eventArgs);
            }
        }

        public void StartHandleDirectory(string dirPath)
        {
            this.m_path = dirPath;
            //  add handler to this directory
            this.m_dirWatcher = new FileSystemWatcher(this.m_path);
            this.m_dirWatcher.Changed += new FileSystemEventHandler(DirectoryCreated);
            this.m_dirWatcher.Created += new FileSystemEventHandler(DirectoryCreated);
            this.m_dirWatcher.EnableRaisingEvents = true;
        }

    }
}
