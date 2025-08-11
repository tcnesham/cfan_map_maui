using Google.OpenLocationCode;
using Maui.GoogleMaps;

namespace CFAN.SchoolMap.Services.PlusCodes
{
    public static class PlusCodeHelper
    {
        public static string ToPlusCode(double latitude, double longitude)
        {
            return OpenLocationCode.Encode(latitude, longitude);
        }

        public static string ToPlusCode(Position latlng)
        {
            return OpenLocationCode.Encode(latlng.Latitude, latlng.Longitude);
        }

        public static Position ToPosition(string plusCode)
        {
            var ca = OpenLocationCode.Decode(plusCode);
            return new Position(ca.CenterLatitude, ca.CenterLongitude);
        }

        public static bool IsValid(string plusCode)
        {
            if (plusCode == null) return false;
            return OpenLocationCode.IsValid(plusCode);
        }
    }
}
