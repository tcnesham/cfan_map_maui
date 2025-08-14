using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using Acr.UserDialogs;
using CFAN.SchoolMap.Enumerations;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.Maui.Model;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.Pins.States;
using CFAN.SchoolMap.Services.Auth;
using CFAN.SchoolMap.Services.Exceptions;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using Plugin.CloudFirestore;
using User = CFAN.SchoolMap.Model.User;
using CFAN.SchoolMap.Database;
using CFAN.SchoolMap.Maui.CountryBorders;
using CFAN.SchoolMap.Maui.Services.Auth;

namespace CFAN.SchoolMap.Maui.Database
{
    public class Repository : IRepository
    {
        public const int ChangesCountLimit = 10;

        public event EventHandler<BasePoint[]>? PlacesChanged;
        public event EventHandler<string>? CountryChanged;

        private IListenerRegistration? _placesListener;
        private IListenerRegistration? _countryListener;
        private CountryPlaces<PlacePoint>? _schoolCurrentCountry;
        private CountryPlaces<MarketPoint>? _marketCurrentCountry;
        private string? _activeListenersCountry;
        private User? _currentUser;
        public static IAuth? Auth { get; set; }
        public static IUserDialogs? Dialogs { get; set; }
        public static IExceptionHandler? ErrorHandler { get; set; }
        public Task Initialization { get; set; }
        public Task UserInitialization { get; set; }

        public User? CurrentUser
        {
            get => _currentUser;
            set
            {
                if (value == _currentUser) return;
                _currentUser = value;
                Auth?.SetCurrentUserRoles(value?.Roles ?? []);
                Notify(nameof(HasSchoolRoles));
                Notify(nameof(HasMarketRoles));
                Notify(nameof(HasAdministrationRoles));
            }
        }


        public Repository()
        {
            Auth = DependencyService.Resolve<IAuth>();
            Dialogs = UserDialogs.Instance;
            ErrorHandler = DependencyService.Resolve<IExceptionHandler>();
            CrossCloudFirestore.Current.Instance.FirestoreSettings = new FirestoreSettings
            {
                CacheSizeBytes = FirestoreSettings.CacheSizeUnlimited
            };
            Initialization = TaskHelper.SafeRun(SaveVersion());
            UserInitialization = TaskHelper.SafeRun(CacheCurrentUser());
        }

        public async Task CacheCurrentUser()
        {
            CurrentUser = await GetCurrentUser();
        }

        public async Task<User> GetCurrentUser()
        {
            if (Auth?.IsSignIn != true) return null!;

            var data = await CrossCloudFirestore.Current.Instance
                .Collection(nameof(User))
                .Document(Auth.User?.Email ?? "")
                .GetAsync();
            if (data.Exists)
            {
                var user = data.ToObject<User>();
                if (user != null)
                {
                    user.Roles ??= [];
                }
                return user!;
            }
            else
            {
                var user = new User()
                {
                    Email = Auth.User?.Email ?? "",
                    Id = Auth.User?.Id ?? "",
                    Name = Auth.User?.Email ?? "",
                    Roles = [ Role.Schools_visit,Role.Schools_add, Role.Schools_view ]
                };
                await AddOrUpdateUser(user);
                return user;
            }
        }

        private void OnCountryDataChanged(CountryData countryData)
        {
            if (countryData==null) return;
            Debug.WriteLine("Country data cash reloaded!");
            CountryChanged?.Invoke(this, countryData.CountryCode);
        }

        private void OnPlacesChanged( PlacePoint[] places)
        {
            if (places.Length == 0) return;
            var country = places.First().Country;
            CountryPlaces<PlacePoint> cp;
            if (country == _schoolCurrentCountry?.CountryCode)
            {
                cp = _schoolCurrentCountry;
            }
            else
            {
                return;
                //load country from disk or from server
            }

            foreach (var p in places)
            {
                cp.Update(p);
            }

            PlacesChanged?.Invoke(this, places);
        }

        private void OnPlacesChanged(MarketPoint[] places)
        {
            if (places.Length == 0) return;
            var country = places.First().Country;
            CountryPlaces<MarketPoint> cp;
            if (country == _marketCurrentCountry?.CountryCode)
            {
                cp = _marketCurrentCountry;
            }
            else
            {
                return;
                //load country from disk or from server
            }

            foreach (var p in places)
            {
                cp.Update(p);
            }

            PlacesChanged?.Invoke(this, places);
        }

        public const string SchoolFBData = "CountryData";
        public const string MarketFBData = "CountryMarketData";

        public async Task<CountryPlaces<PlacePoint>> SchoolsLoadCountry(string countryCode, bool compressData)
        {
            _schoolCurrentCountry = null;
            var data = await CrossCloudFirestore.Current.Instance
                .Collection(SchoolFBData)
                .Document(countryCode)
                .GetAsync();

            if (!data.Exists && !(Auth?.IsTester ?? false))
            {
                if (compressData || await (Dialogs?.ConfirmAsync("Do you really want to add this country to the application?") ?? Task.FromResult(false)))
                {
                    Debug.WriteLine($"Creating {countryCode}");
                    await CrossCloudFirestore.Current.Instance.RunTransactionAsync((t) =>
                    {
                        var dataRef = CrossCloudFirestore.Current.Instance
                            .Collection(SchoolFBData)
                            .Document(countryCode);
                        data = t.Get(dataRef);
                        if (data.Exists) throw new ApplicationException("Country was already created by someone else! Reload the country!");
                        _ = t.Set(dataRef, new CountryData { CountryCode = countryCode, ChangedBy = Auth?.User?.Email ?? "" });
                    });
                    _schoolCurrentCountry = new CountryPlaces<PlacePoint>();
                }
            }

            if (data.Exists)
            {
                var countryData = data.ToObject<CountryData>();
                if (countryData != null)
                {
                    if (countryData != null)
                    {
                        _schoolCurrentCountry = countryData.ToCountryPlaces<PlacePoint>();
                    }
                    else
                    {
                        _schoolCurrentCountry = new CountryPlaces<PlacePoint>();
                    }
                }
                else
                {
                    _schoolCurrentCountry = new CountryPlaces<PlacePoint>();
                }
                var newPlacesDocs = await CrossCloudFirestore.Current.Instance
                    .Collection(nameof(PlacePoint))
                    .WhereEqualsTo(nameof(PlacePoint.Country), countryCode)
                    .GetAsync();

                var newPlaces = newPlacesDocs.ToObjects<PlacePoint>().ToArray();
                //var newPlacesCount = newPlaces.Count(p => p.UpdatedAt > countryData.UpdatedAt);
                if ((Auth?.IsAdmin ?? false)
                    && compressData
                   //&& (lastUpdate.Seconds - countryData.UpdatedAt.Seconds) / 60 / 60 > 2 //at least 2 hours new data
                   )
                {
                    var oldVisits = await CrossCloudFirestore.Current.Instance
                        .Collection(nameof(SchoolVisit))
                        .WhereEqualsTo(nameof(SchoolVisit.Country), countryCode)
                        .WhereEqualsTo(nameof(SchoolVisit.State), PlaceStates.PlaceVisitAllowed.ToString())
                        .WhereLessThan(nameof(SchoolVisit.Date), new Timestamp(DateTime.Today.AddYears(-6)))
                        .GetAsync();
                    if (newPlaces.Length > 0 || oldVisits.Count > 0)
                    {
                        int maxChanges = 450;
                        int changes = 0;
                        int total = newPlaces.Length + oldVisits.Count;
                        try
                        {
                            while (changes < total)
                            {
                                newPlacesDocs = await CrossCloudFirestore.Current.Instance
                                    .Collection(nameof(PlacePoint))
                                    .WhereEqualsTo(nameof(PlacePoint.Country), countryCode)
                                    .LimitTo(maxChanges)
                                    .GetAsync();
                                if (maxChanges > newPlacesDocs.Count)
                                {
                                    oldVisits = await CrossCloudFirestore.Current.Instance
                                        .Collection(nameof(SchoolVisit))
                                        .WhereEqualsTo(nameof(SchoolVisit.Country), countryCode)
                                        .WhereEqualsTo(nameof(SchoolVisit.State),
                                            PlaceStates.PlaceVisitAllowed.ToString())
                                        .WhereLessThan(nameof(SchoolVisit.Date),
                                            new Timestamp(DateTime.Today.AddYears(-5)))
                                        .LimitTo((maxChanges - newPlacesDocs.Count)/2)//delají se dvě operace
                                        .GetAsync();
                                }
                                else
                                {
                                    oldVisits = null;
                                }
                                Debug.WriteLine($"Compressing {countryCode} - new places:{newPlacesDocs.Count}, old visits:{oldVisits?.Count} - ({changes} from {total})");

                                await CrossCloudFirestore.Current.Instance.RunTransactionAsync((t) =>
                                {
                                    changes++;
                                    total++; //saving CountryData
                                    var dataRef = CrossCloudFirestore.Current.Instance
                                        .Collection(SchoolFBData)
                                        .Document(countryCode);
                                    data = t.Get(dataRef);
                                    var countryDataObj = data.ToObject<CountryData>();
                                    _schoolCurrentCountry = countryDataObj != null
                                        ? countryDataObj.ToCountryPlaces<PlacePoint>()
                                        : new CountryPlaces<PlacePoint>();
                                    foreach (var pd in newPlacesDocs.Documents)
                                    {
                                        var p = pd.ToObject<PlacePoint>();
                                        _schoolCurrentCountry.Update(p);
                                        t.Delete(pd.Reference);
                                        changes++;
                                    }

                                    if (oldVisits != null && oldVisits.Count>0)
                                    {
                                        foreach (var vd in oldVisits.Documents)
                                        {
                                            var v = vd.ToObject<SchoolVisit>();
                                            if (v != null)
                                            {
                                                var dr = CrossCloudFirestore.Current.Instance.Document(
                                                    $"{nameof(SchoolVisit) + "Old"}/{v.CreateBackupKey()}");
                                                t.Set(dr, v);
                                                _schoolCurrentCountry?.ChangeToOldVisits(v.PlusCode);
                                                t.Delete(vd.Reference);
                                                changes+=2;
                                            }
                                        }
                                    }

                                    var newCD = CountryData.FromCountryPlaces(_schoolCurrentCountry!);
                                    //kontrola serializace
                                    var cc = newCD?.ToCountryPlaces<PlacePoint>();
                                    if (cc?.PlaceCount != _schoolCurrentCountry?.PlaceCount)
                                        throw new ApplicationException("Serialization corrupted!");
                                    newCD!.ChangedBy = Auth.User?.Email ?? "";
                                    newCD.Version = VersionTracking.CurrentVersion;
                                    t.Set(dataRef, newCD);
                                });
                            }

                            await SaveStatistics(_schoolCurrentCountry);
                        }
                        catch (Exception e)
                        {
                            Dialogs?.Toast("Unable to compress changes! \n" +
                                          e.Message,TimeSpan.FromSeconds(20));
                        }
                    }
                }
                else
                {//only local update
                    foreach (var p in newPlacesDocs.ToObjects<PlacePoint>())
                    {
                        _schoolCurrentCountry.Update(p);
                    }
                }

            }
            if (_activeListenersCountry != countryCode)
            {
                _placesListener?.Remove();
                _placesListener = CrossCloudFirestore.Current.Instance
                    .Collection(nameof(PlacePoint))
                    .WhereEqualsTo(nameof(PlacePoint.Country), countryCode)
                    .WhereGreaterThan(nameof(PlacePoint.UpdatedAt), new Timestamp(DateTime.Now.AddDays(-1)))
                    .AddSnapshotListener((snapshot, error) =>
                    {
                        if (snapshot != null)
                        {
                            try
                            {
                                OnPlacesChanged(snapshot
                                    .DocumentChanges
                                    .Select(s => s.Document.ToObject<PlacePoint>()!)
                                    .Where(p => p != null)
                                    .Cast<PlacePoint>()
                                    .ToArray());
                            }
                            catch
                            {
                                Dialogs?.Toast("Unable to track changes from server! Get online and reload the country!");
                            }
                        }
                    });

                _countryListener?.Remove();
                _countryListener = CrossCloudFirestore.Current.Instance
                    .Collection(SchoolFBData)
                    .WhereEqualsTo(nameof(CountryData.CountryCode), countryCode)
                    .AddSnapshotListener(async (snapshot, error) =>
                    {
                        if (snapshot != null)
                        {
                            try
                            {
                                var countryData = snapshot
                                    .DocumentChanges
                                    .Select(s => s.Document.ToObject<CountryData>())
                                    .FirstOrDefault();
                                if (countryData != null)
                                {
                                    OnCountryDataChanged(countryData);
                                }
                            }
                            catch
                            {
                                Dialogs?.Toast("Unable to track changes from server! Get online and reload the country!");
                            }
                        }
                    });
                _activeListenersCountry = countryCode;
            }
            
            return _schoolCurrentCountry ?? new CountryPlaces<PlacePoint>();
        }

        public async Task<CountryPlaces<MarketPoint>> MarketsLoadCountry(string countryCode, bool compressData)
        {
            _marketCurrentCountry = null;
            var data = await CrossCloudFirestore.Current.Instance
                .Collection(MarketFBData)
                .Document(countryCode)
                .GetAsync();

            if (!data.Exists && !(Auth?.IsTester ?? false))
            {
                if (compressData || await (Dialogs?.ConfirmAsync("Do you really want to add this country to the application?") ?? Task.FromResult(false)))
                {
                    Debug.WriteLine($"Creating {countryCode}");
                    await CrossCloudFirestore.Current.Instance.RunTransactionAsync((t) =>
                    {
                        var dataRef = CrossCloudFirestore.Current.Instance
                            .Collection(MarketFBData)
                            .Document(countryCode);
                        data = t.Get(dataRef);
                        if (data.Exists) throw new ApplicationException("Country was already created by someone else! Reload the country!");
                        t.Set(dataRef, new CountryData { CountryCode = countryCode, ChangedBy = Auth?.User?.Email ?? "" });
                    });
                    _marketCurrentCountry = new CountryPlaces<MarketPoint>();
                }
            }

            if (data.Exists)
            {
                var countryData = data.ToObject<CountryData>();
                if (countryData != null)
                {
                    _marketCurrentCountry = countryData.ToCountryPlaces<MarketPoint>();
                }
                else
                {
                    _marketCurrentCountry = new CountryPlaces<MarketPoint>();
                }
                var newPlacesDocs = await CrossCloudFirestore.Current.Instance
                    .Collection(nameof(MarketPoint))
                    .WhereEqualsTo(nameof(MarketPoint.Country), countryCode)
                    .GetAsync();

                var newPlaces = newPlacesDocs.ToObjects<MarketPoint>().ToArray();
                //var newPlacesCount = newPlaces.Count(p => p.UpdatedAt > countryData.UpdatedAt);
                if ((Auth?.IsAdmin ?? false)
                    && compressData
                   //&& (lastUpdate.Seconds - countryData.UpdatedAt.Seconds) / 60 / 60 > 2 //at least 2 hours new data
                   )
                {
                    if (newPlaces.Length > 0)
                    {
                        int maxChanges = 450;
                        int changes = 0;
                        int total = newPlaces.Length;
                        try
                        {
                            while (changes < total)
                            {
                                newPlacesDocs = await CrossCloudFirestore.Current.Instance
                                    .Collection(nameof(MarketPoint))
                                    .WhereEqualsTo(nameof(MarketPoint.Country), countryCode)
                                    .LimitTo(maxChanges)
                                    .GetAsync();

                                Debug.WriteLine($"Compressing {countryCode} - new places:{newPlacesDocs.Count} - ({changes} from {total})");

                                await CrossCloudFirestore.Current.Instance.RunTransactionAsync((t) =>
                                {
                                    changes++;
                                    total++; //saving CountryData
                                    var dataRef = CrossCloudFirestore.Current.Instance
                                        .Collection(MarketFBData)
                                        .Document(countryCode);
                                    data = t.Get(dataRef);
                                    _marketCurrentCountry = data.ToObject<CountryData>()!.ToCountryPlaces<MarketPoint>();
                                    foreach (var pd in newPlacesDocs.Documents)
                                    {
                                        var p = pd.ToObject<MarketPoint>();
                                        _marketCurrentCountry.Update(p ?? throw new InvalidOperationException("MarketPoint is null"));
                                        t.Delete(pd.Reference);
                                        changes++;
                                    }

                                    var newCD = CountryData.FromCountryPlaces(_marketCurrentCountry);
                                    //kontrola serializace
                                    var cc = newCD.ToCountryPlaces<MarketPoint>();
                                    if (cc.PlaceCount != _marketCurrentCountry.PlaceCount)
                                        throw new ApplicationException("Serialization corrupted!");
                                    newCD!.ChangedBy = Auth.User?.Email ?? "";
                                    newCD.Version = VersionTracking.CurrentVersion;
                                    t.Set(dataRef, newCD);
                                });
                            }

                            //await SaveStatistics(_marketCurrentCountry);
                        }
                        catch (Exception e)
                        {
                            Dialogs?.Toast("Unable to compress changes! \n" +
                                          e.Message, TimeSpan.FromSeconds(20));
                        }
                    }
                }
                else
                {//only local update
                    foreach (var p in newPlacesDocs.ToObjects<MarketPoint>())
                    {
                        _marketCurrentCountry.Update(p);
                    }
                }

            }

            if (_activeListenersCountry != countryCode)
            {
                _placesListener?.Remove();
                _placesListener = CrossCloudFirestore.Current.Instance
                    .Collection(nameof(MarketPoint))
                    .WhereEqualsTo(nameof(MarketPoint.Country), countryCode)
                    .WhereGreaterThan(nameof(MarketPoint.UpdatedAt), new Timestamp(DateTime.Now.AddDays(-1)))
                    .AddSnapshotListener((snapshot, error) =>
                    {
                        if (snapshot != null)
                        {
                            try
                            {
                                OnPlacesChanged(snapshot
                                    .DocumentChanges
                                    .Select(s => s.Document.ToObject<MarketPoint>()!)
                                    .Where(p => p != null)
                                    .Cast<MarketPoint>()
                                    .ToArray());
                            }
                            catch
                            {
                                Dialogs?.Toast("Unable to track changes from server! Get online and reload the country!");
                            }
                        }
                    });

                _countryListener?.Remove();
                _countryListener = CrossCloudFirestore.Current.Instance
                    .Collection(MarketFBData)
                    .WhereEqualsTo(nameof(CountryData.CountryCode), countryCode)
                    .AddSnapshotListener((snapshot, error) =>
                    {
                        if (snapshot != null)
                        {
                            try
                            {
                                var countryData = snapshot
                                    .DocumentChanges
                                    .Select(s => s.Document.ToObject<CountryData>())
                                    .FirstOrDefault();
                                if (countryData != null)
                                {
                                    OnCountryDataChanged(countryData);
                                }
                            }
                            catch
                            {
                                Dialogs?.Toast("Unable to track changes from server! Get online and reload the country!");
                            }
                        }
                    });
                _activeListenersCountry = countryCode;
            }

            return _marketCurrentCountry ?? new CountryPlaces<MarketPoint>();
        }

        private async Task SaveStatistics(CountryPlaces<PlacePoint> currentCountry)
        {
            var stat = currentCountry.GetStatistics();
            await CrossCloudFirestore.Current
                .Instance
                .Collection(nameof(CountryStat))
                .Document(currentCountry.CountryCode)
                .SetAsync(stat);
        }

        public async Task SyncData()
        {
            if (CheckTester()) return;
            bool ok;
            using (Dialogs.Loading("Saving data to the database ..."))
            {
                ok = await CrossCloudFirestore.Current.Instance.WaitForPendingWritesAsync().RunWithTimeout(30);
            }
            if (ok)
            {
                await Dialogs.AlertAsync("All your changes are stored in the database.");
            }
            else
            {
                await Dialogs.AlertAsync("Unable to synchronize data!\n Check your internet connection.");
            }
        }

        public Task<bool> SchoolsCountryHasData(string countryCode)
        {
            return CountryHasData(countryCode, SchoolFBData);
        }

        public Task<bool> MarketsCountryHasData(string countryCode)
        {
            return CountryHasData(countryCode, MarketFBData);
        }
        private async Task<bool> CountryHasData(string countryCode, string collection)
        {
            var data = await CrossCloudFirestore.Current.Instance
                .Collection(SchoolFBData)
                .Document(countryCode)
                .GetAsync();
            return data.Exists;
        }

        public async Task<bool> UserExists(string email)
        {
            var data = await CrossCloudFirestore.Current.Instance
                .Collection(nameof(User))
                .Document(email)
                .GetAsync();
            return data.Exists;
        }

        public async Task AddOrUpdateUser(User user)
        {
            if (CheckTester()) return;

            user.LastEditedByEmail = Auth.User?.Email ?? "";

            await CrossCloudFirestore.Current.Instance
                .Collection(nameof(User))
                .Document(user.Email)
                .SetAsync(user);
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var users = await CrossCloudFirestore.Current.Instance
                .Collection(nameof(User))
                .GetAsync();
            return users.ToObjects<User>();
        }

        public async Task<User> GetUserBy(string email)
        {
            var user = await CrossCloudFirestore.Current.Instance
                .Collection(nameof(User))
                .Document(email)
                .GetAsync();
            var result = user.Exists ? user.ToObject<User>() : null;
            return result ?? new User();
        }
        public async Task<CountryPosition> GetCountryPosition(string countryCode)
        {
            var user = await CrossCloudFirestore.Current.Instance
                .Collection(nameof(CountryPosition))
                .Document(countryCode)
                .GetAsync();
            var position = user.ToObject<CountryPosition>();
            return position ?? new CountryPosition();
        }

        public async Task SetCountryPosition(CountryPosition position)
        {
            if (CheckTester()) return;
            await CrossCloudFirestore.Current.Instance
                .Collection(nameof(CountryPosition))
                .Document(position.CountryCode)
                .SetAsync(position);
        }

        public async Task ResetUserPassword(string email, string pwd)
        {
            if (!Auth.IsAdmin) return;

            try
            {
                await CrossCloudFirestore.Current.Instance
                    .Collection(nameof(ChangePassword))
                    .Document(email)
                    .SetAsync(new ChangePassword { Email = email, Pwd = pwd });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        public async Task DownsizeSchools(string countryCode)
        {
            var dataRef = CrossCloudFirestore.Current.Instance
                .Collection(nameof(CountryData))
                .Document(countryCode);
            var data = await dataRef.GetAsync();

            if (data.Exists)
            {
                var countryData = data.ToObject<CountryData>();
                if (countryData != null)
                {
                    _schoolCurrentCountry = countryData.ToCountryPlaces<PlacePoint>();
                    _schoolCurrentCountry.Downsize();
                    var newCD = CountryData.FromCountryPlaces(_schoolCurrentCountry);
                    newCD.ChangedBy = Auth.User?.Email ?? "";
                    newCD.Version = VersionTracking.CurrentVersion;
                    await dataRef.SetAsync(newCD);
                    await SaveStatistics(_schoolCurrentCountry);
                }
            }
        }

        public async Task SaveVersion()
        {
            if (!Auth.IsSignIn) return;

            var id = Preferences.Get("my_id", string.Empty);
            if (string.IsNullOrWhiteSpace(id))
            {
                id = Guid.NewGuid().ToString();
                Preferences.Set("my_id", id);
            }

            await CrossCloudFirestore.Current
                .Instance
                .Collection(nameof(AppVersion))
                .Document(id)
                .SetAsync(new AppVersion
                {
                    Version = VersionTracking.CurrentVersion, 
                    User = Auth.User?.Email ?? ""
                });
        }

        public async Task SchoolsRepairCountryData(CountryPlaces<PlacePoint> countryPlaces)
        {
            return; // náprava stavů není aktuální
            if (!Auth.IsAdmin) return;
            if (!await Dialogs.ConfirmAsync("Do you want to run data repair?")) return;

            int created = 0;
            int updated = 0;
            int missing = 0;
            int ncreated = 0;
            int screated = 0;

            using var stream = GetType().Assembly.GetManifestResourceStream("CFAN.SchoolMap.BigQueryServiceKey.json");
            var credential = GoogleCredential.FromStream(stream);
            var bqClient = await BigQueryClient.CreateAsync("cfan-schools", credential);
            var statistics = (await bqClient.ExecuteQueryAsync($"select PlusCode, sum(Visit) as Visits from cfan-schools.Schools.Visits where Country='{countryPlaces.CountryCode}'  group by PlusCode", parameters: null))
                .Select(r=>new{PC = r["PlusCode"].ToString(), Visits = int.Parse(r["Visits"].ToString())})
                .ToDictionary(r=>r.PC, r=>r.Visits);

            var allVisits = await CrossCloudFirestore.Current.Instance
                .Collection(nameof(SchoolVisit))
                .WhereEqualsTo(nameof(PlacePoint.Country), countryPlaces.CountryCode)
                .WhereGreaterThan(nameof(PlacePoint.UpdatedAt), new Timestamp(DateTime.Now.AddDays(-30)))
                .GetAsync();

            foreach (var v in allVisits.ToObjects<SchoolVisit>().Where(v=>v.IsAllowed))
            {
                var stat = 0;
                if (statistics.ContainsKey(v.PlusCode))
                {
                    stat = statistics[v.PlusCode];
                }
                if (stat != 1)
                {
                    missing++;
                    var yy = v.Date.ToString("yy");
                    var yymm = v.Date.ToString("yyMM");
                    var yyww = v.Date.ToString("yy") + new GregorianCalendar(GregorianCalendarTypes.Localized).GetWeekOfYear(v.Date, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday).ToString("00");
                    var yymmdd = v.Date.ToString("yyMMdd");
                    Debug.WriteLine($"INSERT Schools.Visits (Country, YY, YYMM, YYWW, YYMMDD, User, Attandance, Decisions, Visit, UpdatedAt, PlusCode) VALUES('{v.Country}', '{yy}', '{yymm}', '{yyww}', '{yymmdd}', '{v.ChangedBy}', {v.NOfChildren}, {(int)(v.NOfChildren * v.PercOfConverts / 100)}, 1, CAST(CURRENT_DATETIME() AS STRING), '{v.PlusCode}'); ");
                }

                var place = countryPlaces.FindPlace(v.PlusCode);
                var state = (v.IsAllowed) ? PlaceStates.Visited
                    : (v.IsNotASchool) ? PlaceStates.Ignored
                    : 'u';
                if (place?.Type == state || (place==null && state=='I')) continue;
                if (place == null)
                {
                    var placeName = await CrossCloudFirestore.Current.Instance
                        .Collection(nameof(PlaceName))
                        .Document(v.PlusCode)
                        .GetAsync();
                    var name = (placeName.Exists) ? placeName.ToObject<PlaceName>()?.N : null;

                    place = new PlacePoint
                    {
                        Country = _schoolCurrentCountry.CountryCode,
                        Name = name,
                        PlusCode = v.PlusCode,
                        Type = state,
                        ChangedBy = Auth.User?.Email ?? ""
                    };
                    created++;
                }
                else
                {
                    place.Type = state;
                    updated++;
                }
                countryPlaces.Update(place);
                await SavePlace(countryPlaces.CountryCode, place);
            }

           var allNames = await CrossCloudFirestore.Current.Instance
                .Collection(nameof(PlaceName))
                .WhereEqualsTo(nameof(PlaceName.CountryCode), countryPlaces.CountryCode)
                .WhereGreaterThan(nameof(PlaceName.UpdatedAt), new Timestamp(DateTime.Now.AddDays(-30)))
                .GetAsync();

            foreach (var v in allNames.Documents)
            {
                var place = countryPlaces.FindPlace(v.Id);
                if (place == null)
                {
                    place = new PlacePoint
                    {
                        Country = _schoolCurrentCountry.CountryCode,
                        Name = v.ToObject<PlaceName>()!.N,
                        PlusCode = v.Id,
                        Type = 'U',
                        ChangedBy = Auth.User?.Email ?? ""
                    };
                    Debug.WriteLine($"{place.PlusCode} - {place.Name}");
                    countryPlaces.Update(place);
                    await SavePlace(countryPlaces.CountryCode, place);
                    ncreated++;
                }
            }

            foreach (var pc in statistics.Keys)
            {
                var stat = statistics[pc];
                if (stat != 1) continue;
                var place = countryPlaces.FindPlace(pc);
                var state = 'V';
                if (place?.Type == 'V') continue;
                if (place == null)
                {
                    var placeName = await CrossCloudFirestore.Current.Instance
                        .Collection(nameof(PlaceName))
                        .Document(pc)
                        .GetAsync();
                    var name = (placeName.Exists) ? placeName.ToObject<PlaceName>()?.N : null;

                    place = new PlacePoint
                    {
                        Country = _schoolCurrentCountry.CountryCode,
                        Name = name,
                        PlusCode = pc,
                        Type = state,
                        ChangedBy = Auth.User?.Email ?? ""
                    };
                    created++;
                }
                else
                {
                    place.Type = state;
                    updated++;
                }
                countryPlaces.Update(place);
                await SavePlace(countryPlaces.CountryCode, place);
            }
            await Dialogs.AlertAsync($"{created} places created, {updated} updated from visits, {missing} missing in statistics. {ncreated} places from names. {screated} places from statistics.");
        }

        public async Task<IEnumerable<PlaceSearchResult>> SearchSchools(string country, string text)
        {
            var strlength = text.Length;
            var to = text.Substring(0, strlength - 1) + (char)(text[strlength-1] + 1);

            var result = await CrossCloudFirestore.Current.Instance
                .Collection(nameof(PlaceName))
                .WhereEqualsTo(nameof(PlaceName.CountryCode), country)
                .WhereGreaterThanOrEqualsTo(nameof(PlaceName.N), text)
                .WhereLessThan(nameof(PlaceName.N), to)
                .LimitTo(10)
                .GetAsync();
            return result.Documents
                .Select(d => d.ToObject<PlaceName>()!)
                .Select(pn => new PlaceSearchResult
                    {Title = pn.N, Subtitle = "CfaN Red school", PlusCode = pn.PlusCode});
        }

        public async Task ImportFromText<TPoint>(string countryCode, string clipboardText)
            where TPoint : BasePoint
        {
//#warning přidat transakci!
//            if (CheckTester()) return;
//            var data = await CrossCloudFirestore.Current.Instance
//                .Collection(nameof(CountryData))
//                .Document(countryCode)
//                .GetAsync();
//            var oldData = data.ToObject<CountryData>();
//            _schoolCurrentCountry = oldData.ToCountryPlaces<PlacePoint>();
//            StringReader strReader = new(clipboardText);
//            while (true)
//            {
//                var line = await strReader.ReadLineAsync();
//                try
//                {
//                    if (line == null) break;
//                    var fields = line.Split('|');
//                    var p = new PlacePoint
//                    {
//                        Type = fields[0][0],
//                        PlusCode = fields[1],
//                        Name = fields[2],
//                        Country = countryCode
//                    };
//                    if (string.IsNullOrEmpty(p.Name))
//                    {
//                        p.Name = null;
//                    }
//                    else
//                    {
//                        await SavePlaceName(p);
//                    }

//                    var saved = _schoolCurrentCountry.FindPlace(p.PlusCode);
//                    if (saved == null)//place does not exists
//                    {

//                        if (p.Type == PlaceStates.Visited)
//                        {
//                            var v = new SchoolVisit
//                            {
//                                PlusCode = p.PlusCode,
//                                Date = DateTime.ParseExact(fields[3], "yyMMdd", CultureInfo.InvariantCulture)
//                                    .AddHours(12),
//                                Note = fields[4],
//                                State = PlaceStates.PlaceVisitAllowed,
//                                NOfChildren = 0,
//                                PercOfConverts = 70
//                            };
//                            await SaveVisit(countryCode, v);
//                        }

//                        _schoolCurrentCountry.Update(p);
//                    }
//                }
//                catch (Exception e)
//                {
//                    ErrorHandler.HandleException(e, false, null, "Error on line " + line);
//                }
                
//            }
//            var newData = CountryData.FromCountryPlaces(_schoolCurrentCountry);
//            newData.UpdatedAt = oldData.UpdatedAt;
//            await CrossCloudFirestore.Current
//                .Instance
//                .Collection(CountryDataCollection<TPoint>())
//                .Document(countryCode)
//                .SetAsync(newData);
//            await Clipboard.SetTextAsync(String.Empty);
        }

        private bool CheckTester()
        {
            if (Auth?.IsTester == true)
            {
                Dialogs?.Toast("You are a tester!\n Your changes will not be stored into the database.");
                return true;
            }

            return false;
        }

        public async Task SavePlace<TPoint>(string countryCode, TPoint place)
            where TPoint : BasePoint
        {
            if (CheckTester()) return;

            var collection = typeof(TPoint).Name;

            place.Country = countryCode;
            place.ChangedBy = Auth?.User?.Email ?? string.Empty;
            await CrossCloudFirestore.Current
                .Instance
                .Collection(collection)
                .Document(place.PlusCode)
                .SetAsync(place);

            await CrossCloudFirestore.Current
                .Instance
                .Collection(collection + "Backup")
                .Document(place.CreateBackupKey())
                .SetAsync(place);
        }

        public async Task SavePlaceName(BasePoint place)
        {
            if (CheckTester()) return;
            if (!place.HasName) return;

            await CrossCloudFirestore.Current
                .Instance
                .Collection(nameof(PlaceName))
                .Document(place.PlusCode)
                .SetAsync(new PlaceName{N=place.Name, CountryCode = place.Country, PlusCode = place.PlusCode});
        }

        public async Task LoadPlaceName(BasePoint place)
        {
            if (place.HasName) return;
            place.Name = place.GetUnknownPlaceName();
            try
            {
                var doc = await CrossCloudFirestore.Current
                    .Instance
                    .Collection(nameof(PlaceName))
                    .Document(place.PlusCode)
                    .GetAsync();
                if (doc.Exists)
                {
                    var n = doc.ToObject<PlaceName>();
                    if (n != null)
                    {
                        place.Name = n.N;
                    }
                }
            }
            catch { }
        }

        public async Task SaveVisit(string countryCode, SchoolVisit visit, bool isUpdate = false)
        {
            if (CheckTester()) return;

            visit.Country = countryCode;
            if (!isUpdate) visit.ChangedBy = Auth?.User?.Email ?? string.Empty;
            if (!visit.IsAllowed)
            {
                visit.NOfChildren = 0;
                visit.PercOfConverts = 0;
            }

            await CrossCloudFirestore.Current
                .Instance
                .Collection(nameof(SchoolVisit))
                .Document(visit.PlusCode)
                .SetAsync(visit);
        }

        public async Task<SchoolVisit> GetVisit(PlacePoint place)
        {
            try
            {
                var doc = await CrossCloudFirestore.Current
                    .Instance
                    .Collection(nameof(SchoolVisit))
                    .Document(place.PlusCode)
                    .GetAsync();
                var visit = (doc != null && doc.Exists) ? doc.ToObject<SchoolVisit>() : null;
                return visit ?? new SchoolVisit();
            }
            catch
            {
                // Optionally log the exception
            }

            return new SchoolVisit();
        }

        public async Task SaveMarketInfo(string countryCode, MarketInfo marketInfo, bool isUpdate = false)
        {
            if (CheckTester()) return;
            
            marketInfo.Country = countryCode;
            if (!isUpdate) marketInfo.ChangedBy = Auth?.User?.Email ?? "";
            
            await CrossCloudFirestore.Current
                .Instance
                .Collection(nameof(MarketInfo))
                .Document(marketInfo.PlusCode)
                .SetAsync(marketInfo);
        }
        public async Task<MarketInfo> GetMarketInfo(MarketPoint place)
        {
            try
            {
                var doc = await CrossCloudFirestore.Current
                    .Instance
                    .Collection(nameof(MarketInfo))
                    .Document(place.PlusCode)
                    .GetAsync();

                var marketInfo = (doc != null && doc.Exists) ? doc.ToObject<MarketInfo>() : null;
                return marketInfo ?? new MarketInfo();
            }
            catch { }

            return new MarketInfo();
        }

        public async Task<CountryStat[]> GetStatistics()
        {
            try
            {
                if (Stat == null)
                {
                    var stats = await CrossCloudFirestore.Current
                        .Instance
                        .Collection(nameof(CountryStat))
                        .GetAsync();
                    Stat = stats.ToObjects<CountryStat>().ToArray();
                }

                return Stat;
            }
            catch { }

            return null;
        }

        public CountryStat[]? Stat { get; set; }

        public void WriteDebugNoBorders()
        {
            var totalCount = ISO3166.Country.List.Length;
            var noBorderCount = 0;
            foreach (var c in ISO3166.Country.List)
            {
                if (CountryBorderHelper.GetCountryBorders(c) == null) noBorderCount++;
            }
            Debug.WriteLine($"___DEBUG: {noBorderCount} of {totalCount} countries don't have borders.");
        }

        public bool HasSchoolRoles => (Auth?.IsAdmin ?? false) || (CurrentUser?.HasModulRoles(Modul.Schools) ?? false);

        public bool HasMarketRoles => (Auth?.IsAdmin ?? false) || (CurrentUser?.HasModulRoles(Modul.Markets) ?? false);

        public bool HasAdministrationRoles => (Auth?.IsAdmin ?? false) || (CurrentUser?.HasAdministrationRoles() ?? false);

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void Notify([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
