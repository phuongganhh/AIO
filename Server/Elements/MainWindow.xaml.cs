using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace Elements
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private SimpleTcpServer ServerLogin { get; set; }
        private SimpleTcpServer ServerGame { get; set; }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.ServerLogin = new SimpleTcpServer();
            this.ServerLogin.ClientConnected += ServerLogin_ClientConnected;
            this.ServerLogin.ClientDisconnected += ServerLogin_ClientDisconnected;
            this.ServerLogin.DataReceived += ServerLogin_DataReceived;
            this.ServerLogin.Start(Common.LoginServerLocal);

            
        }

        private void ServerLogin_DataReceived(object sender, Message e)
        {
            e.TcpClient.GetUserByIP()?.SendToServer(e.Data);
        }

        private void ServerLogin_ClientDisconnected(object sender, TcpClient e)
        {
            e.Disconnect();
        }

        private void ServerLogin_ClientConnected(object sender, TcpClient e)
        {
            e.Client.Add(Common.LoginServer);
            //var login = new byte[] { 8, 0, 35, 4, 168, 122, 0, 0 };
            //e.Client.Send(login);
        }
    }
}
