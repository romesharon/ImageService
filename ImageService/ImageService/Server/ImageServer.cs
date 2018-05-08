using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Infrastructure.Enums;
using ImageService.Logging;
using ImageService.Modal;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace ImageService.Server
{
    public class ImageServer
    {
        #region Members
        private IImageController m_controller;
        private ILoggingService m_logging;
        private TcpListener server;
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

            //  create handlers for each directory from the app config
            foreach (string dir in directories)
            {
                this.m_logging.Log(string.Format("directory to listen {0}", dir), Logging.Modal.MessageTypeEnum.INFO);
                CreateHandler(dir);
            }
        }

        /// <summary>
        /// create a handler to the directory.
        /// </summary>
        /// <param name="directory">path of the directory we want to listen</param>
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

        /// <summary>
        /// alert to the listeners that new event has happend
        /// </summary>
        /// <param name="e">command id</param>
        public void SendCommand(CommandRecievedEventArgs e)
        {
            //  sending the command to all handlers
            this.CommandRecieved?.Invoke(this, e);
        }

        /// <summary>
        /// closing the server and remove all the dirctory handlers.
        /// </summary>
        /// <param name="source">directory handler</param>
        /// <param name="e"></param>
        public void CloseServer(object source, DirectoryCloseEventArgs e)
        {
            if (source is IDirectoryHandler directoryHandler)
            {
                this.CommandRecieved -= directoryHandler.OnCommandRecieved;
                directoryHandler.DirectoryClose -= CloseServer; 
            }
        }

        public void Start()
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            // TcpListener server = new TcpListener(port);
            server = new TcpListener(localAddr, 12345);

            // Start listening for client requests.
            server.Start();
            this.m_logging.Log("Waiting for client connections", Logging.Modal.MessageTypeEnum.INFO);

            Task thread = new Task(() =>
            {
                TcpClient client = server.AcceptTcpClient();

                using (NetworkStream stream = client.GetStream())
                using (BinaryReader reader = new BinaryReader(stream))
                using (BinaryWriter writer = new BinaryWriter(stream))
                try
                {
                    this.m_logging.Log("Client connected", Logging.Modal.MessageTypeEnum.INFO);
                    string commandLine = reader.ReadString();
                    this.m_logging.Log("Client send " + commandLine + " ", Logging.Modal.MessageTypeEnum.INFO);
                    string configvalue1 = ConfigurationManager.AppSettings["Handler"];
                    this.m_logging.Log("Handler " + configvalue1 + " ", Logging.Modal.MessageTypeEnum.INFO);
                    writer.Write("helloClient");
                    this.m_logging.Log("After write", Logging.Modal.MessageTypeEnum.INFO);
                }
                catch (Exception e)
                {
                    this.m_logging.Log("Error: " + e.Message, Logging.Modal.MessageTypeEnum.FAIL);
                }

            });
            thread.Start();
        }
    }
}
