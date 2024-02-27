using GMap.NET.WindowsPresentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GMap_WPF.Model
{
    public class MapData : Notifier
    {
        private int Zoom;
        private GMapControl MapProvider;
        public int zoom
        {
            get { return Zoom; }
            set
            {
                if(Zoom != value)
                {
                    Zoom = value;
                    OnPropertyChanged(nameof(zoom));
                }
            }
        }
        public GMapControl mapProvider
        {
            get { return MapProvider; }
            set
            {

            }
        }
    }
}
