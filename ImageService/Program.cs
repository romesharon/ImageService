using ImageService.Controller;
using ImageService.Controller.Handlers;
using ImageService.Logging;
using ImageService.Infrastructure.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace ImageService
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            /*Modal.ImageServiceModal imgService = new Modal.ImageServiceModal("C:\\Users\\user1\\Desktop\\Images",120);
            bool o = false;
            imgService.AddFile("C:\\Users\\user1\\Pictures\\f.jpg", out o);

            ImageController imageController = new ImageController(imgService);
            LoggingService loggingService = new LoggingService();
            IDirectoryHandler handler = new DirectoyHandler(imageController, loggingService);
            string[] temp = { "C:\\Users\\user1\\Pictures\\unnamed.jpg" };
            handler.OnCommandRecieved(null, new Modal.CommandRecievedEventArgs((int)CommandEnum.NewFileCommand, temp, "C:\\Users\\user1\\Desktop\\Images"));
            */

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new ImageService(args)
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
