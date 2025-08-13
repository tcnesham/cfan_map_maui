using CFAN.SchoolMap.Maui.GoogleMaps.Internals;

using System;

namespace CFAN.SchoolMap.Maui.GoogleMaps
{
    public sealed class CameraUpdate
    {
        public CameraUpdateType UpdateType { get; }
        public Position Position { get; }
        public double Zoom { get; }
        public Bounds Bounds { get; }
        public int Padding { get; }
        public CameraPosition CameraPosition { get; }

        internal CameraUpdate(Position position)
        {
            UpdateType = CameraUpdateType.LatLng;
            Position = position;
        }

        internal CameraUpdate(Position position, double zoomLv)
        {
            UpdateType = CameraUpdateType.LatLngZoom;
            Position = position;
            Zoom = zoomLv;
        }

        internal CameraUpdate(Bounds bounds, int padding)
        {
            UpdateType = CameraUpdateType.LatLngBounds;
            Bounds = bounds;
            Padding = padding;
        }

        internal CameraUpdate(CameraPosition cameraPosition)
        {
            UpdateType = CameraUpdateType.CameraPosition;
            CameraPosition = cameraPosition;
        }

    }
}