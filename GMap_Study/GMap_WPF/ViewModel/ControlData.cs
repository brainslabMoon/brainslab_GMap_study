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
        readonly string filePath = "markers.json";

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
        public GMapMarker selectedMarker
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


        List<GMapRoute> routeArray = new List<GMapRoute>();
        List<GMapMarker> markerArray = new List<GMapMarker>();


        GMapProvider[] mapProviers = new GMapProvider[]
        {
            GMapProviders.GoogleSatelliteMap,
            GMapProviders.GoogleMap,
            GMapProviders.GoogleTerrainMap,
        };
    }
}
