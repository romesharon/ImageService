using ImageServiceGUI.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageServiceGUI.Model
{
    public class ModelMain : IModelMain
    {
        private bool connected;

        public ModelMain()
        {
            this.connected = (ClientSock.Instance == null) ? false : true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public bool Connected
        {
            get
            {
                return this.connected;
            }
            set
            {
                connected = value;
                NotifyPropertyChanged("Connected");
            }
        }

        public void NotifyPropertyChanged(string v)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }
    }
}
