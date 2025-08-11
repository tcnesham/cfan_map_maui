using CFAN.SchoolMap.Database;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.Maui;
using CFAN.SchoolMap.Maui.Database;
using CFAN.SchoolMap.Maui.Model;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using ISO3166;
using Maui.GoogleMaps;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Polygon = GeoJSON.Net.Geometry.Polygon;
using Position = Maui.GoogleMaps.Position;

namespace CFAN.SchoolMap.Maui.CountryBorders
{
    public static class CountryBorderHelper
    {
        private static readonly HashSet<string> _loadedCountries = [];
        private static readonly Dictionary<string, List<Polygon>> _bordersCache = [];
        private static readonly Dictionary<string, Bounds> _boundsCache = [];
        private static readonly Dictionary<string, Position> _centerCache = [];

        public static Task<List<Polygon>?> GetCountryBorders(Country country)
        {
            return GetCountryBorders(country.ThreeLetterCode);
        }
        public static async Task<List<Polygon>?> GetCountryBorders(string country)
        {
            await CacheCountry(country);
            if (!_loadedCountries.Contains(country)) return null;
            return _bordersCache[country];
        }
        public static Task<Bounds?> GetCountryBounds(Country country)
        {
            return GetCountryBounds(country.ThreeLetterCode);
        }

        public static async Task<Bounds?> GetCountryBounds(string country)
        {
            await CacheCountry(country);
            if (!_loadedCountries.Contains(country)) return null;
            return _boundsCache[country];
        }

        public static Task<Position> GetCountryPosition(Country country)
        {
            return GetCountryPosition(country.ThreeLetterCode);
        }

        public static async Task<Position> GetCountryPosition(string country)
        {
            await CacheCountry(country);
            if (!_loadedCountries.Contains(country)) return new Position();
            return _centerCache[country];
        }

        private static IRepository? _repository;

        private static async Task CacheCountry(string country)
        {
            _repository ??= DependencyService.Get<IRepository>();
            if (country.IsNullOrWhiteSpace()) return;
            if (_loadedCountries.Contains(country)) return;

            var polygons = new List<Polygon>();
            var json = ReadCountryGeoJson(country);
            if (json != null)
            {
                var countryPolygons = JsonConvert.DeserializeObject<FeatureCollection>(json);
                var geometry = countryPolygons?.Features?.FirstOrDefault()?.Geometry;
                if (geometry is Polygon gp)
                {
                    polygons.Add(gp);
                }
                else if (geometry is MultiPolygon mp)
                {
                    polygons.AddRange(mp.Coordinates);
                }

                _bordersCache.Add(country, polygons);

                double la_min = double.MaxValue;
                double la_max = double.MinValue;
                double lo_min = double.MaxValue;
                double lo_max = double.MinValue;
                foreach (var point in polygons.SelectMany(p => p.Coordinates.SelectMany(l => l.Coordinates)))
                {
                    if (la_min > point.Latitude) la_min = point.Latitude;
                    if (la_max < point.Latitude) la_max = point.Latitude;
                    if (lo_min > point.Longitude) lo_min = point.Longitude;
                    if (lo_max < point.Longitude) lo_max = point.Longitude;
                }

                var bounds = new Bounds(new Position(la_min, lo_min), new Position(la_max, lo_max));
                _boundsCache.Add(country, bounds);

                _centerCache.Add(country, new Position((la_min + la_max) / 2, (lo_min + lo_max) / 2));
            }
            else
            {
                // Don't add null values to cache - the getter methods handle missing entries
                var pos = await _repository.GetCountryPosition(country);
                if (pos != null)
                {
                    _centerCache.Add(country, new Position(pos.Latitude, pos.Longitude));
                }
                else
                {
                    var place = await GetCountryCoordinatesAsync(country);
                    if (place != null)
                    {
                        await _repository.SetCountryPosition(new CountryPosition
                        { CountryCode = country, Latitude = place.Item1, Longitude = place.Item2 });
                        _centerCache.Add(country, new Position(place.Item1, place.Item2));
                    }
                }
            }
            _loadedCountries.Add(country);
        }

        static async Task<Tuple<double, double>?> GetCountryCoordinatesAsync(string alpha3Code)
        {
            try
            {
                string url = $"https://restcountries.com/v3.1/alpha/{alpha3Code}";

                using HttpClient client = new();
                HttpResponseMessage response = await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();
                    JArray countryData = JArray.Parse(json);

                    if (countryData.Count > 0)
                    {
                        var country = countryData[0];
                        var latlng = country["latlng"];

                        if (latlng != null && latlng.HasValues && latlng.Count() >= 2)
                        {
                            try
                            {
                                var latToken = latlng[0];
                                var lngToken = latlng[1];

                                if (latToken != null && lngToken != null)
                                {
                                    double latitude = (double)latToken;
                                    double longitude = (double)lngToken;
                                    return Tuple.Create(latitude, longitude);
                                }
                            }
                            catch (FormatException)
                            {
                                // Handle invalid format
                            }
                        }
                    }
                }
            }
            catch { }
            return null;
        }

        private static string? ReadCountryGeoJson(string country)
        {
            if (country == null) return null;
            var assembly = typeof(App).Assembly;
            var stream = assembly.GetManifestResourceStream($"CFAN.SchoolMap.CountryBorders.{country}.geo.json");
            if (stream == null)
            {
                //UserDialogs.Instance.Toast("Sorry, we have no border definition for this country.");
                return null;
            }

            using var reader = new StreamReader(stream);
            return reader.ReadToEnd();
        }
    }
}
