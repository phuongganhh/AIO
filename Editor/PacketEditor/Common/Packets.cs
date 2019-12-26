using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PacketEditor.Common
{
    public class Packets
    {
        public byte[] Data { get; set; }
        public string Type { get; set; }
        public string DataString
        {
            get
            {
                return Encoding.UTF8.GetString(this.Data);
            }
        }
    }
}
