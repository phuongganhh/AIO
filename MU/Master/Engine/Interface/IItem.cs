using Master.Engine.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Master.Engine.Interface
{
    public interface IItem
    {
        UIElement Control { get; set; }
        Thickness Margin { get; set; }
        List<IItem> Childrens { get; set; }
        bool State { get; set; }
    }
}
