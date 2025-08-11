using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CFAN.SchoolMap.Maui.Model
{
    public class AppVersion
    {
        public string Version { get; set; }
        [ServerTimestamp]
        public Timestamp UpdatedAt { get; set; }
        public string User { get; set; }
    }
}
