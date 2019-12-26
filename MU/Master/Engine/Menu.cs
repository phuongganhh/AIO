using Master.Engine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Master.Engine
{
    public class Menu : IMenu
    {
        private static Menu _instance { get; set; }
        public static Menu Instance {
            get
            {
                return _instance ?? (_instance = new Menu());
            }
            set
            {
                _instance = value;
            }
        }
        public Menu()
        {
            this.Items = new List<IItem>();
        }
        public List<IItem> Items { get;set; }
        public int HeightElement { get
            {
                return 30;
            }
        }
        public int Width { get;set; }
        public Brush BackgroundColor { get;set; }
        public double Opacity { get;set; }
        public UIElement Title { get;set; }
        public bool LiveRender { get;set; }
        public Brush TextColor { get;set; }
        public StackPanel Panel
        {
            get
            {
                return new StackPanel
                {
                    Width = this.Width,
                    Opacity = this.Opacity,
                    Background = this.BackgroundColor,
                    Height = this.Items.Where(x => x.Margin != null && x.Margin.Left <= Menu.Instance.Width).Count() * 20,
                    Margin = new Thickness(this.Position.X, this.Position.Y, 0, 0),
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
            }
        }
        public MainWindow Window { get; set; }
        public Point Position { get; set; }

        public void Render()
        {
            
            this.Window.mainControl.Children.RemoveRange(0, this.Window.mainControl.Children.Count);
            this.Window.mainControl.Children.Add(this.Panel);

            
            foreach (var item in this.Items)
            {
                try
                {
                    var label = (Label)item.Control;
                }
                catch (Exception)
                {

                }
                finally
                {
                    this.Window.mainControl.Children.Add(item.Control);
                }
            }
        }

    }
}
