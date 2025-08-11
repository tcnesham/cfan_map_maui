using Acr.UserDialogs;
using CFAN.SchoolMap.Database;
using CFAN.SchoolMap.Maui;
using CFAN.SchoolMap.Maui.Database;
using CFAN.SchoolMap.Services.Places;
using CFAN.SchoolMap.Services.PlusCodes;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using GoogleApi;
using GoogleApi.Entities.Common.Enums;
using GoogleApi.Entities.Places.AutoComplete.Request;
using GoogleApi.Entities.Places.Common;
using GoogleApi.Entities.Places.Details.Request;
using GoogleApi.Entities.Places.Search.Common.Enums;
using GoogleApi.Entities.Places.Search.NearBy.Request;
using GoogleApi.Entities.Places.Search.NearBy.Response;
using GoogleApi.Entities.Places.Search.Text.Request;
using GoogleApi.Entities.Places.Search.Text.Response;
using Maui.GoogleMaps;
using static GoogleApi.GooglePlaces;

[assembly: Dependency(typeof(PlaceService))]

namespace CFAN.SchoolMap.Services.Places
{
    public class PlaceService : IPlaceService
    {
        private int _searchCount = 0;

        public PlaceService()
        {
        }

        public async Task<Prediction[]> AutocompleteSearch(double latitude, double longitude, string text, string session, string country)
        {
            await LimitSearch();
            var request = new PlacesAutoCompleteRequest
            {
                Input = text,
                Key = App.Key,
                Radius = 50000,
                Location = new GoogleApi.Entities.Common.Coordinate(latitude, longitude),
                SessionToken = session,
                Components = new[]{new KeyValuePair<Component, string>(Component.Country, country.ToLower()), },
                Language = Language.English
            };

            var response = await GoogleApi.GooglePlaces.AutoComplete.QueryAsync(request);
            if (response.Status == Status.Ok && response.Predictions.Any())
            {
                return response.Predictions.ToArray();
            }
            return null;
        }

        private async Task LimitSearch()
        {
            _searchCount++;
            if (_searchCount >= 20)
            {
                await UserDialogs.Instance.AlertAsync("We pay for searches in the application. Please use them wisely. Thank you.");

                if (_credential == null)
                {
                    using var stream = GetType().Assembly.GetManifestResourceStream("CFAN.SchoolMap.BigQueryServiceKey.json");
                    if (stream == null) throw new Exception("Missing BigQueryServiceKey.json.");
                    _credential = GoogleCredential.FromStream(stream);
                }
                _bqClient = await BigQueryClient.CreateAsync("cfan-schools", _credential);
                _repository ??= DependencyService.Get<IRepository>();
                var user = _repository.CurrentUser;
                await _bqClient.ExecuteQueryAsync($"insert into cfan-schools.Schools.Searches(Date, Email, Name) values (CURRENT_DATETIME(), '{user.Email}', '{user.Name}')", parameters: null);
                _searchCount = 0;
            }
        }

        public async Task<IEnumerable<SearchResult>> NearBySearch(MapSpan area, bool shortSearch, params SearchPlaceType[] types)
        {
            var list = new List<SearchResult>();
            foreach (var type in types)
            {
                var rv = (shortSearch)
                    ?await ShortNearbySearch(type, area)
                    :await FullNearBySearch(type, area);
                list.AddRange(rv);
            }
            return list;
        }

        private async Task<IEnumerable<SearchResult>> ShortNearbySearch(SearchPlaceType type, MapSpan area)
        {
            await LimitSearch();
            var r = new List<SearchResult>();
            if (!_searchIsRunning)
            {
                _searchIsRunning = true;
                try
                {
                    //GooglePlaces.AutoComplete
                       var response = await Search.NearBySearch.QueryAsync(FirstNearBySearchRequest(type, area));
                    if (response.Status == Status.Ok && response.Results.Any())
                    {
                        r.AddRange(ToResult(response));
                    }
                }
                finally
                {
                    _searchIsRunning = false;
                }
            }

            return r;
        }

        public async Task<IEnumerable<SearchResult>> FullNearBySearch(SearchPlaceType type, MapSpan area)
        {
            var r = new List<SearchResult>();
            if (!_searchIsRunning)
            {
                _searchIsRunning = true;
                try
                {
                    using (UserDialogs.Instance.Loading("searching for places"))
                    {
                        await LimitSearch();
                        var response = await Search.NearBySearch.QueryAsync(FirstNearBySearchRequest(type, area));
                        while (response.Status == Status.Ok && response.Results.Any())
                        {
                            r.AddRange(ToResult(response));
                            if (response.NextPageToken != null && response.Results.Count() == 20)
                            {
                                bool invalid;
                                do
                                {
                                    invalid = false;
                                    Thread.Sleep(1000);
                                    try
                                    {
                                        await LimitSearch();
                                        response = await Search.NearBySearch.QueryAsync(NextNearBySearchRequest(response));
                                        invalid = response.Status == Status.InvalidRequest;
                                    }
                                    catch (Exception e)
                                    {
                                        invalid = e.Message.Contains("InvalidRequest");
                                        if (!invalid) throw;
                                    }
                                } while (invalid);

                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
                catch(Exception e)
                {
                    DependencyService.Resolve<Exceptions.IExceptionHandler>().HandleException(e,false);
                }
                finally
                {
                    _searchIsRunning = false;
                }
            }
            return r;
        }
        private IEnumerable<SearchResult> ToResult(PlacesNearbySearchResponse response)
        {
            foreach (var r in response.Results)
            {
                yield return new SearchResult(r.Name, r.Geometry.Location.Latitude, r.Geometry.Location.Longitude, response.NextPageToken);
            }
        }

        private PlacesNearBySearchRequest FirstNearBySearchRequest(SearchPlaceType type, MapSpan area)
        {
            var loc = area.Center;
            return new PlacesNearBySearchRequest
            {
                Key = App.Key,
                Location = new GoogleApi.Entities.Common.Coordinate(loc.Latitude, loc.Longitude),
                Radius = Math.Min(area.Radius.Meters, 30000),
                Type = type
                //,Rankby = Ranking.Distance
            };
        }

        private PlacesNearBySearchRequest NextNearBySearchRequest(PlacesNearbySearchResponse r)
        {
            return new PlacesNearBySearchRequest
            {
                Key = App.Key,
                PageToken = r.NextPageToken
            };
        }

        private IEnumerable<SearchResult> ToResult(PlacesTextSearchResponse response)
        {
            foreach (var r in response.Results)
            {
                yield return new SearchResult(r.Name, r.Geometry.Location.Latitude, r.Geometry.Location.Longitude, response.NextPageToken);
            }
        }

        private bool _searchIsRunning;
        private GoogleCredential _credential;
        private BigQueryClient _bqClient;
        private IRepository _repository;

        //nepoužívat - drahé
        //private async Task<IEnumerable<SearchResult>> ShortTextSearch(string text, MapSpan area)
        //{
        //    var r = new List<SearchResult>();
        //    if (!_searchIsRunning)
        //    {
        //        _searchIsRunning = true;
        //        try
        //        {
        //            var response = await GooglePlaces.Search.TextSearch.QueryAsync(FirstTextSearchRequest(text, area));
        //            if (response.Status == Status.Ok && response.Results.Any())
        //            {
        //                r.AddRange(ToResult(response));
        //            }
        //        }
        //        finally { _searchIsRunning = false; }
        //    }
        //    return r;
        //}

        //nepoužívat - drahé
        //public async Task<IEnumerable<SearchResult>> ShortTextSearch(string text)
        //{
        //    if (text == null) return null;
        //    var r = new List<SearchResult>();
        //    if (!_searchIsRunning)
        //    {
        //        _searchIsRunning = true;
        //        try
        //        {
        //            var response = await GooglePlaces.Search.TextSearch
        //                .QueryAsync(FirstTextSearchRequest(text, MapSpan.FromCenterAndRadius(new Position(0, 0), new Distance(23e6))));
        //            if (response.Status == Status.Ok && response.Results.Any())
        //            {
        //                r.AddRange(ToResult(response));
        //            }
        //        }
        //        finally { _searchIsRunning = false; }
        //    }
        //    return r;
        //}

        //nepoužívat - drahé
        //public async Task<IEnumerable<SearchResult>> FullTextSearch(string text, MapSpan area)
        //{
        //    using (UserDialogs.Instance.Loading("searching for schools"))
        //    {
        //        var r = new List<SearchResult>();
        //        var response = await GooglePlaces.Search.TextSearch.QueryAsync(FirstTextSearchRequest(text, area));
        //        while (response.Status == Status.Ok && response.Results.Any())
        //        {
        //            r.AddRange(ToResult(response));
        //            if (response.NextPageToken != null && response.Results.Count() == 20)
        //            {
        //                bool invalid;
        //                do
        //                {
        //                    invalid = false;
        //                    Thread.Sleep(1000);
        //                    try
        //                    {
        //                        response = await GooglePlaces.Search.TextSearch.QueryAsync(NextSearchRequest(response));
        //                        invalid = response.Status == Status.InvalidRequest;
        //                    }
        //                    catch (Exception e)
        //                    {
        //                        invalid = e.Message.Contains("InvalidRequest");
        //                        if (!invalid) throw;
        //                    }
        //                } while (invalid);

        //            }
        //            else
        //            {
        //                break;
        //            }
        //        }
        //        return r;
        //    }
        //}

        private static PlacesTextSearchRequest FirstTextSearchRequest(string text, MapSpan area)
        {
            return new PlacesTextSearchRequest
            {
                Key = App.Key,
                Location = new GoogleApi.Entities.Common.Coordinate(area.Center.Latitude, area.Center.Longitude),
                Radius = Math.Min(area.Radius.Meters, 49999),
                Query = text,
                Language = Language.English
                //, Type = SearchPlaceType.School
            };
        }

        private static PlacesTextSearchRequest NextSearchRequest(PlacesTextSearchResponse response)
        {
            var r = new PlacesTextSearchRequest
            {
                Key = App.Key,
                PageToken = response.NextPageToken
            };
            return r;
        }

        public async Task<PlacesTextSearchResponse> SearchNext(string pageToken)
        {
            await LimitSearch();
            var r = new PlacesTextSearchRequest
            {
                Key = App.Key,
                PageToken = pageToken
            };
            return await Search.TextSearch.QueryAsync(r);
        }

        public async Task<string> GetPluscodeOfPlace(string placeId)
        {
            await LimitSearch();
            var r = new PlacesDetailsRequest
            {
                Key = App.Key,
                PlaceId = placeId
            };
            var rv= await GooglePlaces.Details.QueryAsync(r);
            return PlusCodeHelper.ToPlusCode(new Position(rv.Result.Geometry.Location.Latitude, rv.Result.Geometry.Location.Longitude));
        }
    }
}
//namespace GoogleApi.Entities.Places.Search.Text.Response
//{
//    public class MyPlacesTextSearchResponse : PlacesTextSearchResponse { }

//    public class MyPlacesTextSearchRequest:PlacesTextSearchRequest
//    {
//        public override IList<KeyValuePair<string, string>> GetQueryStringParameters()
//        {
//            var parameters = base.GetQueryStringParameters();
//            parameters.Add("rankby", "distance");
//            return parameters;
//        }
//    }
//}

