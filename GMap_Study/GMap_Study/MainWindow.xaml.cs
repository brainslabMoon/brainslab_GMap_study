using System.Text;
using System.Collections.Generic;
using System.Windows;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GMap.NET;
using GMap.NET.WindowsPresentation;
using GMap.NET.MapProviders;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;

namespace GMapStudy
{
    public partial class MainWindow : Window
    {
        int currentIdx = 0;
        object[] mapProviers = new object[]
        {
            GMapProviders.GoogleSatelliteMap,
            GMapProviders.GoogleMap,
            GMapProviders.GoogleTerrainMap,
        };

        List<PointLatLng> markers = new List<PointLatLng>();
        List<GMapRoute> routeArray = new List<GMapRoute>();
        List<GMapMarker> markerArray = new List<GMapMarker>();

        GMapMarker selectedMarker;
        int selectedIndex;

        readonly string filePath = "markers.json";

        public MainWindow()
        {
            NativeMethods.AllocConsole();

            InitializeComponent();
            InitGMap();

            Closing += MainWindow_Closing;

        }

        private void InitGMap()
        {
            mapControl.MapProvider = GMapProviders.GoogleKoreaSatelliteMap;
            mapControl.Position = new PointLatLng(35.164928, 128.127485);
            mapControl.MinZoom = 2;
            mapControl.MaxZoom = 20;
            mapControl.Zoom = 15;
            mapControl.ShowCenter = false;
            mapControl.DragButton = MouseButton.Left;

            mapControl.MouseDoubleClick += new MouseButtonEventHandler(MapControlMouseLeftButtonDown);
            mapControl.MouseLeftButtonUp += new MouseButtonEventHandler(MapControlMouseUp);
            mapControl.MouseWheel += MapControl_MouseWheel;
            currentIdx++;

            ReadJson();
            DrawRoute();
        }

        private void ReadJson()
        {
            if (File.Exists(filePath))
            {
                try
                {
                    string jsonString = File.ReadAllText(filePath);

                    JObject json = JObject.Parse(jsonString);
                    JArray markersArray = (JArray)json["markers"];

                    foreach (JObject pointObject in markersArray)
                    {
                        double lat = (double)pointObject["Lat"];
                        double lng = (double)pointObject["Lng"];
                        markers.Add(new PointLatLng(lat, lng));
                    }


                    foreach (var point in markers)
                    {
                        Console.WriteLine($"Latitude: {point.Lat}, Longitude: {point.Lng}");
                    }

                    if (markers.Count > 0)
                    {
                        foreach (var point in markers)
                        {
                            LoadMarker(point);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }


        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (markers.Count > 0)
            {
                SaveJson(markers);
            }
        }


        private void SaveJson(List<PointLatLng> markers)
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
            File.WriteAllText("markers.json", jsonString);
        }

        private void MapControl_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            sliderAdjustZoom.Value = mapControl.Zoom;
        }

        private void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedMarker != null && e.LeftButton == MouseButtonState.Pressed)
            {
                var position = e.GetPosition(mapControl);
                var newPosition = mapControl.FromLocalToLatLng((int)position.X, (int)position.Y);
                selectedMarker.Position = newPosition;

                int index = markerArray.IndexOf(selectedMarker);
                if (index != -1)
                {
                    markers[index] = newPosition;
                }

                UpdateRoute();
            }
        }

        private void UpdateRoute()
        {
            foreach (var route in routeArray)
            {
                mapControl.Markers.Remove(route);
            }
            routeArray.Clear();

            RefreshRoute();
        }

        private void RefreshRoute()
        {

            var points = markers.Select(marker => new PointLatLng(marker.Lat, marker.Lng)).ToList();
            if (points.Count > 1)
            {
                GMapRoute route = new GMapRoute(points);
                route.Shape = new System.Windows.Shapes.Path()
                {
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 4
                };
                route.Offset = new Point(6, 6);
                mapControl.Markers.Add(route);
                routeArray.Add(route);
            }
        }

        private void MarkerMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            mapControl.CanDragMap = false;

            FrameworkElement element = (FrameworkElement)sender;
            selectedMarker = (GMapMarker)element.DataContext;
            selectedIndex = (int)mapControl.Markers.IndexOf(selectedMarker) / 2;

        }



        private void MapControlMouseUp(object sender, MouseButtonEventArgs e)
        {
            mapControl.CanDragMap = true;
            selectedMarker = null;
        }



        private void MapControlMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(mapControl);
            PointLatLng point = mapControl.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);

            DrawMarker(point);
            DrawRoute();

        }

        private void LoadMarker(PointLatLng point)
        {
            GMapMarker marker = new GMapMarker(point);

            var temp = new Ellipse
            {
                Width = 15,
                Height = 15,
                Fill = Brushes.Red
            };
            temp.MouseDown += MarkerMouseLeftButtonDown;
            temp.MouseUp += MapControlMouseUp;
            marker.Shape = temp;

            mapControl.Markers.Add(marker);
            markerArray.Add(marker);
        }

        private void DrawMarker(PointLatLng point)
        {
            GMapMarker marker = new GMapMarker(point);

            var temp = new Ellipse
            {
                Width = 15,
                Height = 15,
                Fill = Brushes.Red
            };
            temp.MouseDown += MarkerMouseLeftButtonDown;
            temp.MouseUp += MapControlMouseUp;
            marker.Shape = temp;

            mapControl.Markers.Add(marker);
            markerArray.Add(marker);
            markers.Add(point);

        }

        private void DrawRoute()
        {

            GMapRoute route = new GMapRoute(markers);
            route.Shape = new System.Windows.Shapes.Path()
            {
                Stroke = new SolidColorBrush(Colors.Red),
                StrokeThickness = 4
            };

            route.Offset = new Point(6, 6);
            mapControl.Markers.Add(route);
            routeArray.Add(route);

        }

        private void btn_changeMapProvider_Click(object sender, RoutedEventArgs e)
        {
            if (currentIdx < mapProviers.Length)
            {
                mapControl.MapProvider = (GMapProvider)mapProviers[currentIdx];
                currentIdx++;
            }

            else
            {
                currentIdx = 0;

                mapControl.MapProvider = (GMapProvider)mapProviers[currentIdx];
                currentIdx++;
            }
        }

        private void sliderAdjustZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            mapControl.Zoom = (int)sliderAdjustZoom.Value;
        }

        static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AllocConsole();
        }
    }
}