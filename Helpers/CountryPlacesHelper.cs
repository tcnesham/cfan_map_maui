using CFAN.SchoolMap.Maui.Model;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.Pins;

namespace CFAN.SchoolMap.Helpers
{
    public static class CountryPlacesHelper
    {
        public static PlacePoint ChangeToOldVisits(this Maui.Model.CountryPlaces<PlacePoint> baseCountryPlaces, string plusCode)
        {
            baseCountryPlaces.TryGetValue(plusCode, out var place);
            if (place != null && place.IsVisited)
            {
                place.Type = PinDesignFactory.WithOldVisit.TypeCh;
                place.TeamChar = '0';
                return place;
            }

            return null;
        }

        public static Maui.Model.CountryStat GetStatistics(this CountryPlaces<PlacePoint> baseCountryPlaces)
        {
            var rv = new CountryStat
            {
                CountryCode = baseCountryPlaces.CountryCode,
                Schools = baseCountryPlaces.Places.Count(p => p.IsSchool),
                VisitedSchools = baseCountryPlaces.Places.Count(p => p.IsVisited)
            };
            return rv;
        }
    }
}
