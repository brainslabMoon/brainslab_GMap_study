using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET.WindowsPresentation;
using GMap.NET;
using System.Windows;
using System.Windows.Input;

namespace GMap_WPF
{
    public class ExtensionGMapControl : GMapControl
    {
        public MouseButton dragButton
        {
            get { return DragButton; }
            set
            {
                if (DragButton != value)
                {
                    DragButton = value;
                }
            }
        }

        public bool showCenter
        {
            get { return ShowCenter; }
            set
            {
                if (ShowCenter != value)
                {
                    ShowCenter = value;
                }
            }
        }
    }
}
