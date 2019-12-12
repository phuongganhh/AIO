using SimpleTCP;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;
using System.Windows;
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
            try
            {
                this.LoginFunc();
                this.GameFunc();
                Common.Packet.DataGrid = this.dataPacket;
                this.btnStop.IsEnabled = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LoginFunc()
        {
            this.ServerLogin = new SimpleTcpServer();
            this.ServerLogin.ClientConnected += ServerLogin_ClientConnected;
            this.ServerLogin.ClientDisconnected += ServerLogin_ClientDisconnected;
            this.ServerLogin.DataReceived += ServerLogin_DataReceived;
            this.ServerLogin.Start(Common.LoginServerLocal);
        }
        private void GameFunc()
        {
            this.ServerGame = new SimpleTcpServer();
            this.ServerGame.ClientConnected += ServerGame_ClientConnected;
            this.ServerGame.ClientDisconnected += ServerGame_ClientDisconnected;
            this.ServerGame.DataReceived += ServerGame_DataReceived;
            this.ServerGame.Start(Common.GameServer);
        }

        private void ServerGame_DataReceived(object sender, Message e)
        {
            e.TcpClient.GetUser()?.SendToServer(e.Data);
            this.Dispatcher.Invoke(() =>
            {
                if (this.search.Text == "")
                {
                    e.Data.SetPacket();
                }
                else
                {
                    try
                    {
                        int size = Convert.ToInt32(this.search.Text);
                        if (e.Data.Length == size)
                        {
                            e.Data.SetPacket();
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            });
        }

        private void ServerGame_ClientDisconnected(object sender, TcpClient e)
        {
            e.Disconnect();
        }

        private void ServerGame_ClientConnected(object sender, TcpClient e)
        {
            e.Client.Add(Common.GameServer);
        }

        private void ServerLogin_DataReceived(object sender, Message e)
        {
            e.TcpClient.GetUser()?.SendToServer(e.Data);
            
        }

        private void ServerLogin_ClientDisconnected(object sender, TcpClient e)
        {
            e.Disconnect();
        }

        private void ServerLogin_ClientConnected(object sender, TcpClient e)
        {
            e.Client.Add(Common.LoginServer);
        }

        private void ButtonStart_Click(object sender, RoutedEventArgs e)
        {
            Common.Packet.isStart = true;
            Common.Packet.isLock = true;
            if (Common.Packet.isLock)
            {
                this.btnLock.Content = "UnLock";
            }
            else
            {
                this.btnLock.Content = "Lock";
            }
            this.Dispatcher.Invoke(() =>
            {
                this.btnStart.IsEnabled = false;
                this.btnStop.IsEnabled = true;
            });
            Common.StartAuto();
        }

        private void ButtonLock_Click(object sender, RoutedEventArgs e)
        {
            Common.Packet.isLock = !Common.Packet.isLock;
            if (Common.Packet.isLock)
            {
                this.btnLock.Content = "UnLock";
            }
            else
            {
                this.btnLock.Content = "Lock";
            }
        }

        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {
            Common.Packet.Data = new List<byte[]>();
            Common.Packet.Reload();
        }

        private void ButtonStop_Click(object sender, RoutedEventArgs e)
        {
            Common.Packet.isStart = false;
            this.Dispatcher.Invoke(() =>
            {
                this.btnStart.IsEnabled = true;
                this.btnStop.IsEnabled = false;
            });
        }

        private void ButtonExit_Click(object sender, RoutedEventArgs e)
        {
            this.ServerGame.Stop();
            this.ServerLogin.Stop();
            Common.Exit();
            Environment.Exit(0);
        }
    }
}
