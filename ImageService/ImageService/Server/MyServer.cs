using ImageService.Controller;
using ImageService.Logging;
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
    class MyServer
    {
        private ImageServer imageServer;
        private IImageController imageController;
        private ILoggingService loggingService;
        private int port;
        private TcpListener tcpListener;
        private IClient clientHandler;

        public MyServer(ImageServer imageServer,  IImageController imageController, ILoggingService loggingService, int port)
        {
            this.imageServer = imageServer;
            this.imageController = imageController;
            this.loggingService = loggingService;
            this.port = port;
            this.clientHandler = new Client(this.loggingService, this.imageController);
        }

        public void Start()
        {
            this.loggingService.Log("Server Started", Logging.Modal.MessageTypeEnum.INFO);
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");

            this.tcpListener = new TcpListener(localAddr, this.port);
            this.tcpListener.Start();
            // Start listening for client requests.
            this.loggingService.Log("Waiting for client connections", Logging.Modal.MessageTypeEnum.INFO);

            Task thread = new Task(() =>
            {
                while (true)
                {
                    try
                    {
                        TcpClient client = this.tcpListener.AcceptTcpClient();
                        this.loggingService.Log("Client connected", Logging.Modal.MessageTypeEnum.INFO);
                        this.clientHandler.AddClient(client);
                    }
                    catch (Exception e)
                    {
                        this.loggingService.Log("Error: " + e.Message, Logging.Modal.MessageTypeEnum.FAIL);
                        break;
                    }
                }

            });
            thread.Start();
        }
    }
}