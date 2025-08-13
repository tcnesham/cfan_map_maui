using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Map
{
    public static class XLocationExtensions
    {
        public static string ToGpsString(this Location l)
        {
            return l.Latitude.ToString().Replace(',', '.') + "," + l.Longitude.ToString().Replace(',', '.');
        }
    }
}