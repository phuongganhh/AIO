using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Controls;

namespace Elements
{
    public class Packet
    {
        public Packet()
        {
            this.Data = new List<byte[]>();
            this.isLock = false;
            this.isStart = false;
        }
        public List<byte[]> Data { get; set; }
        public bool isLock { get; set; }
        public bool isStart { get; set; }
        public DataGrid DataGrid { get; set; }

        public void Reload()
        {
            this.DataGrid.Dispatcher.Invoke(() =>
            {
                this.DataGrid.ItemsSource = this.Data.Select(x =>
                {
                    var s = "";
                    foreach (var item in x)
                    {
                        s += item + " ";
                    }
                    return new
                    {
                        Size = x.Length,
                        Data = s
                    };
                });
            });
        }

        public void SetPacket(byte[] data)
        {
            if (!this.isLock)
            {
                this.Data.Add(data);
                this.Reload();
            }
        }
    }
    public static class Common
    {
        public static string IPServer = "137.74.253.26";
        public static string IPLocal = "127.0.0.1";
        public static int LoginServer = 37955;
        public static int LoginServerLocal = 9958;
        public static int GameServer = 5840;
        public static int HackProtection = 56999;
        public static Packet Packet { get; set; } = new Packet();
        public static void StartAuto()
        {
            if(Users.Count > 0)
            {
                foreach (var u in Users)
                {
                    new Thread((usr) =>
                    {
                        var user = (User)usr;
                        while (Packet.isStart)
                        {
                            foreach (var item in Packet.Data)
                            {
                                user.SendToServer(item);
                                Thread.Sleep(1500);
                            }
                        }
                    }).Start(u);
                }
            }
        }

        public static void SetPacket(this byte[] data)
        {
            Packet.SetPacket(data);
        }
        public static void Send(this TcpClient e,byte[] data)
        {
            e.Client.Send(data);
        }
        private static List<User> Users { get; set; } = new List<User>();
        public static User GetUser(this TcpClient e)
        {
            return Users.FirstOrDefault(x => x.ToString() == e.Client.RemoteEndPoint.ToString());
        }
        public static void Exit()
        {
            foreach (var item in Users)
            {
                item.DisconnectToServer();
                item.Client.Disconnect(false);
            }
        }
        private static void RemoveUser(User u)
        {
            Users = Users.Where(x => x.ToString() != u.ToString()).ToList();
        }
        public static void Disconnect(this TcpClient e)
        {
            var user = e.GetUser();
            user?.DisconnectToServer();
           if(user != null)
            {
                RemoveUser(user);
            }
        }

        public static void Add(this Socket socket,int? port = null)
        {
            Users.Add(new User(socket, port));
        }

        private static int FindBytes(byte[] src, byte[] find)
        {
            int index = -1;
            int matchIndex = 0;
            // handle the complete source array
            for (int i = 0; i < src.Length; i++)
            {
                if (src[i] == find[matchIndex])
                {
                    if (matchIndex == (find.Length - 1))
                    {
                        index = i - matchIndex;
                        break;
                    }
                    matchIndex++;
                }
                else if (src[i] == find[0])
                {
                    matchIndex = 1;
                }
                else
                {
                    matchIndex = 0;
                }

            }
            return index;
        }

        private static byte[] ReplaceBytes(byte[] src, byte[] search, byte[] repl)
        {
            byte[] dst = null;
            int index = FindBytes(src, search);
            if (index >= 0)
            {
                dst = new byte[src.Length];
                // before found array
                Buffer.BlockCopy(src, 0, dst, 0, index);
                // repl copy
                Buffer.BlockCopy(repl, 0, dst, index, repl.Length);
                // rest of src array
                Buffer.BlockCopy(
                    src,
                    index + search.Length,
                    dst,
                    index + repl.Length,
                    src.Length - (index + search.Length));
            }
            return dst ?? src;
        }
        public static byte[] Replace(this byte[] src, byte[] search, byte[] repl)
        {
           return  ReplaceBytes(src, search, repl);
        }
    }
    
}
