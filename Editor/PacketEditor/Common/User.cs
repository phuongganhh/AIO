using PacketEditor.Common;
using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace PacketEditor
{
    public class User
    {
        public User(Socket sk)
        {
            this.MySelf = sk;
            this.lstPacket = new List<Packets>();
            this.Client = new SimpleTcpClient();
            this.Client.DataReceived += Client_DataReceived;
            this.Client.Connect("", 1);
        }
        public Socket MySelf { get; set; }
        private void Client_DataReceived(object sender, Message e)
        {
            this.MySelf.Send(e.Data);
            this.lstPacket.Add(new Packets
            {
                Data = e.Data,
                Type = Setting.Recv
            });
        }

        private SimpleTcpClient Client { get; set; }
        public List<Packets> lstPacket { get; set; }
    }
}
