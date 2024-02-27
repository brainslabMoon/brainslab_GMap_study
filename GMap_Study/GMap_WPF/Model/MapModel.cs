using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GMap;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace GMap_WPF.Model
{
    public class MapModel
    {
        public MapModel()
        {
            GMap.NET.MapProviders.GMapProvider provider = GMapProviders.GoogleKoreaSatelliteMap;
        }
    }
}
