namespace CFAN.SchoolMap.Services.Places
{
    public class SearchResult
    {
        public SearchResult(string name, double latitude, double longitude, string nextPageToken)
        {
            Name = name;
            Latitude = latitude;
            Longitude = longitude;
            NextPageToken = nextPageToken;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string Name { get; set; }
        public string NextPageToken { get; set; }
    }
}