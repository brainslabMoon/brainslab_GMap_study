using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GMap_WPF.Model
{
    public class MapData : Notifier
    {
        private int Zoom;
        public int zoom
        {
            get { return Zoom; }
            set
            {
                if(Zoom != value)
                {
                    Zoom = value;
                    OnPropertyChanged(nameof(zoom));
                }
            }
        }

    }
}
