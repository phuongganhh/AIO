using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Master.Engine.Interface
{
    public interface IMenu
    {
        Point Position { get; set; }
        List<IItem> Items { get; set; }
        int Width { get; set; }
        Brush BackgroundColor { get; set; }
        double Opacity { get; set; }
        UIElement Title { get; set; }
        bool LiveRender { get; set; }
        Brush TextColor { get; set; }
        StackPanel Panel { get; }
        MainWindow Window { get; set; }
        void Render();
    }
}
