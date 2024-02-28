using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using System.Windows.Input;

namespace GMap_WPF.Model
{
    public class MapData : Notifier
    {
        private GMapProvider? MapProvider;
        public GMapProvider mapProvider
        {
            get { return MapProvider; }
            set
            {
                if (MapProvider != value)
                {
                    MapProvider = value;
                }
            }
        }

        private PointLatLng Position;
        public PointLatLng position
        {
            get { return Position; }
            set
            {
                if (position != value)
                {
                    Position = value;
                }
            }
        }

        private int MinZoom = 0;
        public int minZoom
        {
            get { return MinZoom; }
            set
            {
                if (MinZoom != value)
                {
                    MinZoom = value;
                }
            }
        }

        private int MaxZoom = 0;
        public int maxZoom
        {
            get { return MaxZoom; }
            set
            {
                if (MaxZoom != value)
                {
                    MaxZoom = value;
                }
            }
        }

        private int Zoom = 0;
        public int zoom
        {
            get { return Zoom; }
            set
            {
                if (Zoom != value)
                {
                    Zoom = value;
                    OnPropertyChanged(nameof(zoom));
                }
            }
        }

        private bool ShowCenter = false;
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

        private MouseButton DragButton;
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
    }
}
