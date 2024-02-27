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


namespace GMap_WPF.ViewModel
{
    public class MainViewVM
    {
        private readonly MapModel mapModel;

        public MainViewVM(MapModel mapModel)
        {
            this.mapModel = mapModel;
        }   
    }
}
