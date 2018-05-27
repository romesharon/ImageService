using ImageService.Logging.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ImageService.Server
{
    /// <summary>
    /// Interface IClient
    /// </summary>
    interface IClient
    {
        /// <summary>
        /// Adds the client.
        /// </summary>
        /// <param name="tcpClient">The TCP client.</param>
        void AddClient(TcpClient tcpClient);
        /// <summary>
        /// Sends the message to all clients.
        /// </summary>
        /// <param name="message">The message.</param>
        void SendMessageToAllClients(string message);
        /// <summary>
        /// Sends the log to all clients.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="args">The <see cref="MessageRecievedEventArgs"/> instance containing the event data.</param>
        void SendLogToAllClients(object sender, MessageRecievedEventArgs args);
    }
}
