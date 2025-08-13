using CFAN.SchoolMap.Maui.GoogleMaps;
using Microsoft.Maui.ApplicationModel;

namespace CFAN.SchoolMap.Map
{
    public static class GLocationExtensions
    {
        
        public static Position ToPosition(this GoogleApi.Entities.Common.Coordinate l)
        {
            return new Position(l.Latitude, l.Longitude);
        }
        
    }
}
