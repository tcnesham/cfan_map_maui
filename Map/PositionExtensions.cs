using GoogleApi.Entities.Common;
using CFAN.SchoolMap.Maui.GoogleMaps;

namespace CFAN.SchoolMap.Maui
{
    public static class PositionExtensions
    {
        
        public static Coordinate ToLocation(this Position p)
        {
            return new Coordinate(p.Latitude, p.Longitude);
        }
        
    }
}