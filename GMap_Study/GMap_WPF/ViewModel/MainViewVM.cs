using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;
using GMap_WPF.Model;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;


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

        private ControlData _controlData;
        public ControlData controlData
        {
            get { return _controlData; }
            set { _controlData = value; }
        }


        public MainViewVM()
        {
            // Model 객체 및 ControlData 객체 생성
            mapData = new MapData();
            controlData = new ControlData();

            mapData.mapProvider = GMapProviders.GoogleKoreaSatelliteMap;
            mapData.position = new PointLatLng(35.164928, 128.127485);
            mapData.zoom = 15;


            controlData.currentMapProviderIdx++;
        }

        public void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (controlData.markers.Count > 0)
            {
                SavetoJson(controlData.markers);
            }

            ClearAllResource();
        }

        private void ClearAllResource()
        {
            if(controlData.mapControl.Markers.Count > 1)
            {
                ClearMarkers();
            }
       
            controlData.mapControl.Dispose();
            Application.Current.Shutdown();
        }

        private void ClearMarkers()
        {
            foreach(GMapMarker marker in controlData.markerArray)
            {
                controlData.mapControl.Markers.Remove(marker);
            }

            controlData.markerArray.Clear();
            controlData.markers.Clear();
        }

        private void ReadJson()
        {
            if (File.Exists(controlData.filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(controlData.filePath);

                    JObject json = JObject.Parse(jsonString);

                    if (json != null)
                    {
                        JArray markersArray = (JArray)json["markers"];

                        if (markersArray != null)
                        {
                            foreach (JObject pointObject in markersArray)
                            {
                                double lat = (double)pointObject["Lat"];
                                double lng = (double)pointObject["Lng"];
                                controlData.markers.Add(new PointLatLng(lat, lng));
                            }

                            if (controlData.markers.Count > 0)
                            {
                                foreach (var point in controlData.markers)
                                {
                                    GMapMarker marker = DrawMarker(point);

                                    AddMarkerOnMap(marker);
                                    AddMarkerArray(marker);
                                }
                                DrawRoute(controlData.markers);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void SavetoJson(List<PointLatLng> markers)
        {
            var json = new JObject();

            JArray pointsArray = new JArray();
            foreach (var point in markers)
            {
                JObject pointObject = new JObject();
                pointObject.Add("Lat", point.Lat);
                pointObject.Add("Lng", point.Lng);
                pointsArray.Add(pointObject);
            }

            json.Add("markers", pointsArray);

            string jsonString = json.ToString();

            using (StreamWriter file = File.CreateText("markers.json"))
            {
                file.Write(jsonString);
            }
        }

        public void mapControl_Loaded(object sender, RoutedEventArgs e)
        {
            controlData.mapControl = sender as GMapControl;

            ReadJson();
        }

        public void btnChangeMapProvider_Click()
        {

            if (controlData.currentMapProviderIdx < controlData.mapProviers.Length)
            {
                mapData.mapProvider = controlData.mapProviers[controlData.currentMapProviderIdx];
                controlData.currentMapProviderIdx++;
            }
            else
            {
                controlData.currentMapProviderIdx = 0;

                mapData.mapProvider = controlData.mapProviers[controlData.currentMapProviderIdx];
                controlData.currentMapProviderIdx++;
            }
        }

        public void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (controlData.selectedMarker != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(controlData.mapControl);
                var newPosition = controlData.mapControl.FromLocalToLatLng((int)position.X, (int)position.Y);

                controlData.selectedMarker.Position = newPosition;

                int index = controlData.markerArray.IndexOf(controlData.selectedMarker);
                if (index != -1)
                {
                    controlData.markers[index] = newPosition;
                }

                UpdateRoute();
            }
        }

        private void UpdateRoute()
        {
            foreach (var route in controlData.routeArray)
            {
                controlData.mapControl.Markers.Remove(route);
            }
            controlData.routeArray.Clear();

            RefreshRoute();
        }

        private void RefreshRoute()
        {
            var points = controlData.markers.Select(marker => new PointLatLng(marker.Lat, marker.Lng)).ToList();
            if (points.Count > 1)
            {
                DrawRoute(points);
            }
        }


        public void mapControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (controlData.mapControl != null)
            {
                Point clickPoint = e.GetPosition(controlData.mapControl);
                PointLatLng point = controlData.mapControl.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);

                GMapMarker marker = DrawMarker(point);

                AddMarkerOnMap(marker);
                AddMarkerArray(marker);
                AddMarkerPosArray(point);

                DrawRoute(controlData.markers);
            }
        }

        private GMapMarker DrawMarker(PointLatLng point)
        {
            GMapMarker marker = new GMapMarker(point);


            var grid = new Grid()
            {
                Width = 20,
                Height = 20,
                Background = Brushes.Transparent,
            };
            grid.MouseDown += MarkerMouseLeftButtonDown;
            grid.MouseUp += MarkerMouseUp;

            var temp = new Ellipse
            {
                Width = 15,
                Height = 15,
                Fill = Brushes.Red
            };

            grid.Children.Add(temp);
            marker.Shape = grid;

            return marker;
        }

        private void DrawRoute(List<PointLatLng> markers)
        {
            GMapRoute route = new GMapRoute(markers);
            route.Shape = new System.Windows.Shapes.Path()
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 4
            };
            route.Offset = new Point(10, 10);

            AddRouteOnMap(route);
            AddRouteArray(route);
        }

        private void AddRouteOnMap(GMapRoute route)
        {
            controlData.mapControl.Markers.Add(route);
        }

        private void AddRouteArray(GMapRoute route)
        {
            controlData.routeArray.Add(route);
        }


        private void AddMarkerOnMap(GMapMarker marker)
        {
            controlData.mapControl.Markers.Add(marker);
        }

        private void AddMarkerArray(GMapMarker marker)
        {
            controlData.markerArray.Add(marker);
        }

        private void AddMarkerPosArray(PointLatLng point)
        {
            controlData.markers.Add(point);
        }

        private void MarkerMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            controlData.mapControl.CanDragMap = false;

            FrameworkElement element = (FrameworkElement)sender;
            controlData.selectedMarker = (GMapMarker)element.DataContext;

        }
        public void MarkerMouseUp(object sender, MouseButtonEventArgs e)
        {
            controlData.mapControl.CanDragMap = true;
            controlData.selectedMarker = null;
        }
    }
}
