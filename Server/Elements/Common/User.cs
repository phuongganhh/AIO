using SimpleTCP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Elements
{
    public class User : IDisposable
    {
        public Socket Client { get; set; }
        private SimpleTcpClient ConnectToServer { get; set; }
        public User(Socket client, int? Port = null)
        {
            this.Client = client;
            this.ConnectToServer = new SimpleTcpClient();
            this.ConnectToServer.DataReceived += ConnectToServer_DataReceived;
            this.ConnectToServer.Connect(Common.IPServer, Port ?? Common.LoginServer);
        }
        public void SendToServer(byte[] data)
        {
            this.ConnectToServer.Write(data);
        }
        private void ConnectToServer_DataReceived(object sender, Message e)
        {
            this.Client.Send(e.Data);
        }
        public void DisconnectToServer()
        {
            this.ConnectToServer.Disconnect();
        }
        public override string ToString()
        {
            return this.Client.RemoteEndPoint.ToString();
        }
        public void Dispose()
        {

        }
    }
}
