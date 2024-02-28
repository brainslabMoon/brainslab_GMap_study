using GMap.NET.MapProviders;
using GMap.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GMap.NET.WindowsPresentation;
using System.Runtime.CompilerServices;
using GMap_WPF.Model;
using System.Windows.Controls;
using System.Windows;


namespace GMap_WPF.ViewModel
{
    public class MainViewVM : Notifier
    {
        private MapData _mapData;
        public MapData mapData
        {
            get { return _mapData; }
            set { _mapData = value; }
        }

        public MainViewVM()
        {
            mapData = new MapData();
            mapData.mapProvider = GMapProviders.GoogleKoreaSatelliteMap;
            mapData.position = new PointLatLng(35.164928, 128.127485);
            mapData.minZoom = 2;
            mapData.maxZoom = 20;
            mapData.zoom = 15;
            
            mapData.currentMapProviderIdx++;
        }

        public void btnChangeMapProvider_Click()
        {
            MessageBox.Show("버튼 눌림");




        }
    }
}
