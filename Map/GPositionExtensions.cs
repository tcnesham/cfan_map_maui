using CFAN.SchoolMap.Maui.GoogleMaps;
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Map
{
    public static class GPositionExtensions
    {
        public static string ToGpsString(this Position l)
        {
            return l.Latitude.ToString().Replace(',', '.') + "," + l.Longitude.ToString().Replace(',', '.');
        }
    }
}