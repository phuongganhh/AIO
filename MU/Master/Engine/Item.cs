using Master.Engine.Enum;
using Master.Engine.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Master
{
    public class Item : IItem
    {
        public Item() { }
        public Item(UIElement element, Thickness thickness)
        {
            this.Control = element;
            this.Margin = thickness;
        }
        public Item(UIElement element)
        {
            this.Control = element;
        }

        public UIElement Control { get;set; }
        public List<IItem> Childrens { get;set; }
        public bool State { get;set; }
        public Thickness Margin { get; set; }
        public bool GetState()
        {
            try
            {
                var button = (Button)this.Control;
                return button.Content.ToString() == "ON";
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
