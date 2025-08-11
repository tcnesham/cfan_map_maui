using System.ComponentModel;
using CFAN.SchoolMap.Database;
using CFAN.SchoolMap.Maui.Model;
using CFAN.SchoolMap.Model;

namespace CFAN.SchoolMap.Maui.Database
{
    public interface IRepository : INotifyPropertyChanged
    {
        event EventHandler<BasePoint[]> PlacesChanged;
        event EventHandler<string>  CountryChanged;

        Task<CountryPlaces<PlacePoint>> SchoolsLoadCountry(string countryCode, bool compressData);
        Task<CountryPlaces<MarketPoint>> MarketsLoadCountry(string countryCode, bool compressData);
        Task SavePlace<TPoint>(string countryCode, TPoint place) where TPoint : BasePoint;
        Task SaveVisit(string countryCode, SchoolVisit visit, bool isUpdate = false);
        Task<SchoolVisit> GetVisit(PlacePoint place);
        Task SaveMarketInfo(string countryCode, MarketInfo marketVisit, bool isUpdate = false);
        Task<MarketInfo> GetMarketInfo(MarketPoint place);
        Task SavePlaceName(BasePoint place);
        Task LoadPlaceName(BasePoint place);
        Task ImportFromText<TPoint>(string countryCode, string clipboardText) where TPoint : BasePoint;
        Task<CountryStat[]> GetStatistics();
        Task SchoolsRepairCountryData(CountryPlaces<PlacePoint> countryPlaces);
        Task<IEnumerable<PlaceSearchResult>> SearchSchools(string country, string text);
        Task SyncData();
        Task<bool> SchoolsCountryHasData(string countryCode);
        Task<bool> MarketsCountryHasData(string countryCode);
        Task<bool> UserExists(string email);
        Task AddOrUpdateUser(User user);
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserBy(string email);
        Task<User> GetCurrentUser();
        Task ResetUserPassword(string email, string pwd);
        void WriteDebugNoBorders();
        bool HasSchoolRoles { get; }
        bool HasMarketRoles { get; }
        Task DownsizeSchools(string countryCode);
        bool HasAdministrationRoles { get; }
        User? CurrentUser { get; set; }
        Task<CountryPosition> GetCountryPosition(string countryCode);
        Task SetCountryPosition(CountryPosition position);
    }
}
