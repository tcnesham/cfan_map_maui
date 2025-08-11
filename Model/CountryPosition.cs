using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CFAN.SchoolMap.Maui.Model
{
    public class CountryPosition
    { 
        public string CountryCode { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
