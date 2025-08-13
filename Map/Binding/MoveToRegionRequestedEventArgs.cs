﻿using System;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Bindings
{
    internal sealed class MoveToRegionRequestedEventArgs : EventArgs
    {
        internal MapSpan MapSpan { get; }
        internal bool Animated { get; }

        internal MoveToRegionRequestedEventArgs(MapSpan mapSpan, bool animated)
        {
            MapSpan = mapSpan;
            Animated = animated;
        }
    }
}
