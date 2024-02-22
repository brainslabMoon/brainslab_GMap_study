using System.Text;
using System.Collections.Generic;
using System.Windows;
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
using System.Windows.Controls.Primitives;



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

        PointLatLng start;
        PointLatLng end;

        GMapMarker currentMaker;

        List<PointLatLng> markers = new List<PointLatLng>();


        public MainWindow()
        {
            InitializeComponent();

            InitGMap();

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

            currentIdx++;
        }

        private void MapControlMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point clickPoint = e.GetPosition(mapControl);
            PointLatLng point = mapControl.FromLocalToLatLng((int)clickPoint.X, (int)clickPoint.Y);
            
            GMapMarker marker = new GMapMarker(point);
            marker.Shape = new Ellipse
            {
                Width = 15,
                Height = 15,
                Fill = Brushes.Red
            };
            mapControl.Markers.Add(marker);
            markers.Add(point);

            DrawPath();
        }

        private void DrawPath()
        {
            if (markers.Count > 1)
            {
                GMapRoute route = new GMapRoute(markers);
                route.Shape = new Path()
                {
                    Stroke = new SolidColorBrush(Colors.Red),
                    StrokeThickness = 4
                };
                route.Offset = new Point(6, 6);
                mapControl.Markers.Add(route);

            }         
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
    }
}