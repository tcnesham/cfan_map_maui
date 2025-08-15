using GoogleApi.Entities.Places.Common;
using GoogleApi.Entities.Places.Search.Common.Enums;
using Microsoft.Maui.Maps;

namespace CFAN.SchoolMap.Services.Places
{
    public interface IPlaceService
    {
    Task<IEnumerable<SearchResult>> NearBySearch(MapSpan area, bool shortSearch, params SearchPlaceType[] types);
        Task<Prediction[]> AutocompleteSearch(double latitude, double longitude, string text, string session, string country);
        Task<string> GetPluscodeOfPlace(string placeId);
        //Task<IEnumerable<SearchResult>> ShortTextSearch(string text);
    }
}
