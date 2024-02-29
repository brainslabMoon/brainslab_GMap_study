using GMap.NET.WindowsPresentation;
using System.Windows.Input;

namespace GMap_WPF
{
    public class ExtensionGMapControl : GMapControl
    {

        public ExtensionGMapControl()
        { 
            
        }

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

        public bool canDragMap
        {
            get { return CanDragMap; }
            set
            {
                if (CanDragMap != value)
                {
                    CanDragMap = value;
                }
            }
        }
    }
}
