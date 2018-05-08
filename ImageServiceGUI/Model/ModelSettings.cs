using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace ImageServiceGUI.Model
{
    class ModelSettings : IModelSettings
    {
        string outputDirectory;
        string sourceName;
        string logName;
        int thumbnailSize;
        private ObservableCollection<string> Handlers = new ObservableCollection<string>();

        public string OutputDirectory
        {
            get
            {
                return this.outputDirectory;
            }
            set
            {
                this.outputDirectory = value;
                //notify property changed
            }
        }
        public string SourceName
        {
            get
            {
                return this.sourceName;
            }
            set
            {
                this.sourceName = value;
                //notify property changed
            }
        }
        public string LogName
        {
            get
            {
                return this.logName;
            }
            set
            {
                this.logName = value;
                //notify property changed
            }
        }
        public int ThumbnailSize
        {
            get
            {
                return this.thumbnailSize;
            }
            set
            {
                this.thumbnailSize = value;
                //notify property changed
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void GetInfoFromService()
        {
            //implement.
            Task thread = new Task(() =>
            {
                TcpClient tc = null;
                try
                {
                    tc = new TcpClient();
                    tc.Connect(new System.Net.IPEndPoint(IPAddress.Parse("127.0.0.1"), 12345));

                    using (NetworkStream stream = tc.GetStream())
                    using (BinaryReader reader = new BinaryReader(stream))
                    using (BinaryWriter writer = new BinaryWriter(stream))
                    {
                        // Send command to server
                        writer.Write("GetInfo"); //or something else

                        //// Get result from server
                        ////we can change the order.
                        //this.OutputDirectory = reader.ReadString();
                        //this.SourceName = reader.ReadString();
                        //this.LogName = reader.ReadString();
                        //this.ThumbnailSize = reader.ReadInt32();
                        //Console.WriteLine(OutputDirectory);
                        //Console.WriteLine(SourceName);
                        //Console.WriteLine(LogName);
                        //Console.WriteLine(ThumbnailSize);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                finally
                {
                    //should close ????????? 
                    if (tc != null)
                    {
                        //tc.Close();
                    }
                }
            });
            thread.Start();
        }
    }
}
