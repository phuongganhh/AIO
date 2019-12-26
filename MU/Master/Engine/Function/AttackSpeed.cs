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
    public class AttackSpeed
    {
        private static AttackSpeed _instance { get; set; }
        public static AttackSpeed Instance
        {
            get
            {
                return _instance ?? (_instance = new AttackSpeed());
            }
        }
        public void AGI()
        {
            var text = new Label()
            {
                Content = "Tốc độ đánh",
                Width = Menu.Instance.Width * 0.8,
                Height = Menu.Instance.HeightElement,
                VerticalAlignment = VerticalAlignment.Top,
                HorizontalAlignment = HorizontalAlignment.Left,
                FontWeight = FontWeight.FromOpenTypeWeight(500),
                HorizontalContentAlignment = HorizontalAlignment.Left,
                Foreground = Brushes.White,
                Margin = new Thickness(0, Menu.Instance.Items.Count * Menu.Instance.HeightElement, 0, 0),
                BorderThickness = new Thickness(0, 0, 0, 1),
            };
            var button = new Button()
            {
                Content = "OFF",
                Width = Menu.Instance.Width * 0.2,
                Height = Menu.Instance.HeightElement,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Background = Brushes.Red,
                Margin = new Thickness(text.Width, Menu.Instance.Items.Count * Menu.Instance.HeightElement, 0, 0)
            };
            button.Click += Button_Click;
            Menu.Instance.Items.Add(new Item(text,text.Margin));
            Menu.Instance.Items.Add(new Item(button,button.Margin));
        }
        private IItem Number
        {
            get;
            set;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            
            if (button.Content.ToString() == "OFF")
            {
                button.Content = "ON";
                button.Background = Brushes.Orange;
                var number = new Slider()
                {
                    Width = 200,
                    Height = Menu.Instance.HeightElement,
                    FontSize = 19,
                    Minimum = 0,
                    Maximum = 1000,
                    Margin = new Thickness(button.Margin.Left + button.Width + 5, button.Margin.Top, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };
                number.ValueChanged += Number_ValueChanged;
                this.Number =  new Item(number,number.Margin);
                Menu.Instance.Items.Add(this.Number);

            }
            else
            {
                button.Content = "OFF";
                button.Background = Brushes.Red;
                Menu.Instance.Items.Remove(this.Number);
            }
            Menu.Instance.Render();
        }

        private void Number_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
        }
    }
}
