using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;


namespace GMap_WPF.ViewModel
{
    public class ControlData
    {
        private GMapControl? MapControl;
        public GMapControl mapControl
        {
            get { return MapControl; }
            set
            {
                if(MapControl != value)
                {
                    MapControl = value;
                }
            }
        }

        private readonly string FilePath = "markers.json";
        public  string filePath
        {
            get { return FilePath; }
            set { }
        }

        private int CurrentMapProviderIdx = 0;
        public int currentMapProviderIdx
        {
            get { return CurrentMapProviderIdx; }
            set
            {
                if (CurrentMapProviderIdx != value)
                {
                    CurrentMapProviderIdx = value;
                }
            }
        }

        private GMapMarker? SelectedMarker;
        public GMapMarker? selectedMarker
        {
            get { return SelectedMarker; }
            set
            {
                if(SelectedMarker != value)
                {
                    SelectedMarker = value;
                }
            }
        }


        private List<PointLatLng> Markers = new List<PointLatLng>();
        public List<PointLatLng> markers
        {
            get { return Markers; }
            set { Markers = value; }
        }


        private List<GMapRoute> RouteArray = new List<GMapRoute>();
        public List<GMapRoute> routeArray
        {
            get { return RouteArray; }
            set { RouteArray = value; }
        }

        private List<GMapMarker> MarkerArray = new List<GMapMarker>();
        public List<GMapMarker> markerArray
        {
            get { return MarkerArray; }
            set { MarkerArray = value; }
        }

        private GMapProvider[] MapProviers = new GMapProvider[]
        {
            GMapProviders.GoogleSatelliteMap,
            GMapProviders.GoogleMap,
            GMapProviders.GoogleTerrainMap,
        };
        public GMapProvider[] mapProviers
        {
            get { return MapProviers; }
            set { MapProviers = value; }
        }

    }
}
