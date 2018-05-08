using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    interface IClient
    {
        void AddClient(TcpClient tcpClient);
        void SendMessageToAllClients(MessageRecievedEventArgs args);
    }
}
