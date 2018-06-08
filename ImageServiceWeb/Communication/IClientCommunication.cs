using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImageService.Modal;

namespace WebApplication2.Communication
{
    interface IClientCommunication
    {
        void Send(CommandRecievedEventArgs commandRecievedEventArgs);

        void Close();

        void Recieve();

        bool IsConnected { get; set; }
    }
}
