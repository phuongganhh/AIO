using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;

namespace Elements
{
    public static class Common
    {
        public static string IPServer = "103.27.237.153";
        public static int LoginServer = 9958;
        public static int GameServer = 5816;

        public static void Send(this TcpClient e,byte[] data)
        {
            e.Client.Send(data);
        }
        private static List<User> Users { get; set; } = new List<User>();
        public static User GetUserByIP(this TcpClient e)
        {
            return Users.FirstOrDefault(x => x.ToString() == e.Client.RemoteEndPoint.ToString());
        }
        private static void RemoveUser(User u)
        {
            Users = Users.Where(x => x.ToString() != u.ToString()).ToList();
        }
        public static void Disconnect(this TcpClient e)
        {
            var user = e.GetUserByIP();
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
    }
    
}
