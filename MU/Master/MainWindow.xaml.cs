using Master.Engine;
using Master.Engine.Enum;
using Master.Engine.Interface;
using System;
using System.Diagnostics;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Menu = Master.Engine.Menu;
using Rect = Master.Engine.Rect;

namespace Master
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
        private Point Pixel { get; set; }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {




            this.Pixel = new Point(300, 300);
            var pos = new Point(0, 0);
            Menu.Instance = new Menu()
            {
                LiveRender = true,
                Opacity = 0.7,
                Width = (int)this.Pixel.X,
                BackgroundColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#181d24")),
                Position = pos
            };
            var title = new Label()
            {
                Content = "Welcome",
                Width = this.Pixel.X,
                Height = 30,
                Background = Brushes.Orange,
                Foreground = Brushes.White,
                FontSize = 15,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(pos.X, pos.Y, 0, 0),
                HorizontalAlignment = HorizontalAlignment.Left,
            };
            title.MouseDown += Title_MouseDown;
            Menu.Instance.Items.Add(new Item(title));
            Menu.Instance.Window = this;


            this.LoadMenu();
            Menu.Instance.Render();
        }
        private void SetWindow()
        {
            Process pr = null;
            Rect rect = new Rect();
            while(pr == null)
            {
                Process p = Process.GetProcesses("Main").FirstOrDefault();
                if(p != null)
                {
                    if(p.MainWindowHandle != IntPtr.Zero)
                    {
                        pr = p;
                    }
                }
            }
            Timer timer = new Timer();
            timer.Elapsed += Timer_Elapsed;
            timer.Interval = 1000;
            timer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void LoadMenu()
        {
            AttackSpeed.Instance.AGI();
        }
        
        private void Title_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        
    }
}
