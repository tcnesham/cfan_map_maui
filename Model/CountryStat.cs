using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CFAN.SchoolMap.Maui.Model
{
    public class CountryStat
    {
        public string CountryCode { get; set; }
        public int Schools { get; set; }
        public int VisitedSchools { get; set; }

        [ServerTimestamp]
        public Timestamp UpdatedAt { get; set; }

        [Ignored] public string Visualization => $"{VisitedSchools:## ##0} / {Schools:## ##0}";
    }
}