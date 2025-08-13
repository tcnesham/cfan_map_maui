using System;
using CFAN.SchoolMap.Map;
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Maui.GoogleMaps
{
    public sealed class PoiClickedEventArgs : EventArgs
    {
        public bool Handled { get; set; } = false;
        public Poi Poi { get; }

        internal PoiClickedEventArgs(Poi poi)
        {
            this.Poi = poi;
        }
    }
}