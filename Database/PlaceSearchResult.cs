using System;
using System.Threading.Tasks;

namespace CFAN.SchoolMap.Database
{
    public class PlaceSearchResult
    {
        public string Title { get; set; }
        public string Subtitle { get; set; }
        public Func<Task<string>> GetPlusCodeAction { get; set; }
        public string PlusCode { get; set; }
    }
}