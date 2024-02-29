using GMap.NET;
using GMap.NET.MapProviders;
using System.Diagnostics.Eventing.Reader;

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
                    OnPropertyChanged(nameof(mapProvider));
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
    }
}
