using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Devices.Sensors;
using Polygon = GeoJSON.Net.Geometry.Polygon;

namespace CFAN.SchoolMap.Helpers
{
    public static class GeoJsonPolygonExtensions
    {
        public static IEnumerable<Microsoft.Maui.Controls.Maps.Polygon> ToMapPolygons(this Polygon p)
        {
            foreach (var lineString in p.Coordinates)
            {
                var po = new Microsoft.Maui.Controls.Maps.Polygon
                {
                    StrokeWidth = 2, 
                    FillColor = Color.FromRgba(200,200,200,20),
                    StrokeColor = Colors.Red
                };
                foreach (var coordinate in lineString.Coordinates)
                {
                    po.Geopath.Add(new Location(coordinate.Latitude, coordinate.Longitude));
                }
                yield return po;
            }
        }
    }
}
