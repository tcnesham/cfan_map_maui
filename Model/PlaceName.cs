using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CFAN.SchoolMap.Model
{
    public class PlaceName
    {
        public string? CountryCode { get; set; }
        public string? N { get; set; }
        public string? PlusCode { get; set; }
        [ServerTimestamp]
        public Timestamp UpdatedAt { get; set; }
    }
}
