using SimpleTCP;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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
            if(this.ConnectToServer.TcpClient.Connected)
                this.ConnectToServer.Write(data);
        }
        private void ConnectToServer_DataReceived(object sender, Message e)
        {
            var dataSend = e.Data;
            if (e.MessageString.Contains(Common.IPServer))
            {
                var local = Encoding.UTF8.GetBytes(Common.IPLocal);
                var ele = Encoding.UTF8.GetBytes(Common.IPServer);
                dataSend = e.Data.Replace(ele, local);
            }
            this.Client.Send(dataSend);
        }
        public void DisconnectToServer()
        {
            this.ConnectToServer.Disconnect();
            Thread.Sleep(1000);
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
