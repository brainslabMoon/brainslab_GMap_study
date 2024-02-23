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
using System.Runtime.InteropServices;



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

        MapControl draggingMarker;
        GMapMarker selectedMarker;
        PointLatLng startPoint;


        public MainWindow()
        {
            InitializeComponent();
            NativeMethods.AllocConsole();
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
            mapControl.MouseLeftButtonDown += new MouseButtonEventHandler(MarkerMouseLeftButtonDown);
            mapControl.MouseLeftButtonUp += new MouseButtonEventHandler(MapControlMouseUp);

            currentIdx++;

        }


        private void mapControl_MouseMove(object sender, MouseEventArgs e)
        {
            if(selectedMarker != null && e.LeftButton == MouseButtonState.Pressed)
            {
                Console.WriteLine("마우스 드래그");
                mapControl.CanDragMap = false;


                var position = e.GetPosition(mapControl);
                var newPosition = mapControl.FromLocalToLatLng((int)position.X, (int)position.Y);
                selectedMarker.Position = newPosition;
            }
        }

        private void MapControlMouseUp(object sender, MouseButtonEventArgs e)
        {
            //if(selectedMarker != null)
            //{
            //    PointLatLng endPoint = mapControl.FromLocalToLatLng((int)e.GetPosition(mapControl).X, (int)e.GetPosition(mapControl).Y);
            //    selectedMarker.Position = endPoint;
            //}
            mapControl.CanDragMap = true;
            selectedMarker = null;
        }


        private void MarkerMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            selectedMarker = null;
            if (e.ChangedButton == MouseButton.Left)
            {
                PointLatLng mousePosition = mapControl.FromLocalToLatLng((int)e.GetPosition(mapControl).X, (int)e.GetPosition(mapControl).Y);

                foreach (GMapMarker marker in mapControl.Markers)
                {
                    if (Math.Abs(marker.Position.Lat - mousePosition.Lat) < 0.01 && Math.Abs(marker.Position.Lng - mousePosition.Lng) < 0.01)
                    {
                        selectedMarker = marker;
                        Console.WriteLine(" 마커 클릭");
                        //mapControl.CanDragMap = false;
                        break;

                    }
                }
            }   
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



        static class NativeMethods
        {
            [DllImport("kernel32.dll", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool AllocConsole();
        }

        
    }
}