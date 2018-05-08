using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using ImageService.Controller;
using ImageService.Logging;
using ImageService.Modal;
using ImageService.Server;
using System.Configuration;
using ImageService.Logging.Modal;

namespace ImageService
{
    public partial class ImageService : ServiceBase
    {
        public enum ServiceState
        {
            SERVICE_STOPPED = 0x00000001,
            SERVICE_START_PENDING = 0x00000002,
            SERVICE_STOP_PENDING = 0x00000003,
            SERVICE_RUNNING = 0x00000004,
            SERVICE_CONTINUE_PENDING = 0x00000005,
            SERVICE_PAUSE_PENDING = 0x00000006,
            SERVICE_PAUSED = 0x00000007,
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct ServiceStatus
        {
            public int dwServiceType;
            public ServiceState dwCurrentState;
            public int dwControlsAccepted;
            public int dwWin32ExitCode;
            public int dwServiceSpecificExitCode;
            public int dwCheckPoint;
            public int dwWaitHint;
        };
        private ImageServer m_imageServer;          // The Image Server
        private Modal.IImageServiceModal modal;
        private IImageController controller;
        private ILoggingService logging;
        private int eventId = 1;    //contain the identifier of the next event to write into the event log.

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool SetServiceStatus(IntPtr handle, ref ServiceStatus serviceStatus);

        /// <summary>
        /// ImageService constructor.
        /// </summary>
        /// <param name="args"></param>
        public ImageService(string[] args)
        {
            InitializeComponent();
            //  get the data from the app config
            string logName = ConfigurationManager.AppSettings.Get("LogName");
            int thumbnailSize = Int32.Parse(ConfigurationManager.AppSettings.Get("ThumbnailSize"));
            string sourceName = ConfigurationManager.AppSettings.Get("SourceName");
            string outputDir = ConfigurationManager.AppSettings.Get("OutputDir");
            //we will probably not use it
            if (args.Count() > 0)
            {
                sourceName = args[0];
            }
            if (args.Count() > 1)
            {
                logName = args[1];
            }

            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists(sourceName))
            {
                System.Diagnostics.EventLog.CreateEventSource(sourceName, logName);
            }
            eventLog1.Source = sourceName;
            eventLog1.Log = logName;

            this.modal = new ImageServiceModal(outputDir, thumbnailSize);
            this.controller = new ImageController(modal);

            this.logging = new LoggingService();
            this.logging.MessageRecieved += MessageEvent;
        }

        /// <summary>
        /// write to log.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageEvent(object sender, MessageRecievedEventArgs e)
        {
            this.eventId++;
            string toLog = string.Format("{0}\nreturned with status: {1}\n", e.Message, e.Status);
            eventLog1.WriteEntry(toLog, EventLogEntryType.Information, this.eventId);
        }

        /// <summary>
        /// system calls it when the service start.
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
     
            eventLog1.WriteEntry("In OnStart");
            //create the server
            this.m_imageServer = new ImageServer(controller, logging);
            this.m_imageServer.Start(); // start connection with clients
            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);
        }

        /// <summary>
        /// writes to Log regularly.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        public void OnTimer(object sender, System.Timers.ElapsedEventArgs args)
        {
            // TODO: Insert monitoring activities here.  
            eventLog1.WriteEntry("Monitoring the System", EventLogEntryType.Information, eventId++);
        }

        /// <summary>
        /// system calls it when the service stop.
        /// </summary>
        protected override void OnStop()
        {
            // Update the service state to Start Pending.  
            ServiceStatus serviceStatus = new ServiceStatus();
            serviceStatus.dwCurrentState = ServiceState.SERVICE_START_PENDING;
            serviceStatus.dwWaitHint = 100000;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

            eventLog1.WriteEntry("In onStop.");

            // Set up a timer to trigger every minute.  
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 60000; // 60 seconds  
            timer.Elapsed += new System.Timers.ElapsedEventHandler(this.OnTimer);
            timer.Start();

            // Update the service state to Running.  
            serviceStatus.dwCurrentState = ServiceState.SERVICE_RUNNING;
            SetServiceStatus(this.ServiceHandle, ref serviceStatus);

        }

        /// <summary>
        /// system calls it when the service continue.
        /// </summary>
        protected override void OnContinue()
        {
            eventLog1.WriteEntry("In OnContinue.");
        }
    }
}
