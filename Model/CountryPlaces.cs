using CFAN.SchoolMap.Pins;
using CFAN.SchoolMap.Services.PlusCodes;
using CFAN.SchoolMap.Pins.States;

namespace CFAN.SchoolMap.Maui.Model
{
    public class CountryPlaces<PointT> where PointT : BasePoint, new()
    {
        protected readonly Dictionary<string, PointT> _places = new();
        

        public string CountryCode { get; set; }
        public IEnumerable<PointT> Places => _places.Values;

        public int PlaceCount => _places.Values.Count;

        public PointT Add(PinDesign design, string plusCode)
        {
            _places.TryGetValue(plusCode, out var place);
            if (place != null)
            {
                place.Type = design.TypeCh;
            }
            else
            {
                place = new PointT
                {
                    PlusCode = plusCode,
                    Type = design.TypeCh
                };
                _places.Add(plusCode, place);
            }
            return place;
        }

        public PointT? ChangePlaceType(string plusCode, PinDesign design, char teamChar='0')
        {
            _places.TryGetValue(plusCode, out var place);
            if (place != null)
            {
                place.Type = design.TypeCh;
                place.TeamChar = teamChar;
                return place;
            }

            return null;
        }

        public bool ContainsPlace(string plusCode)
        {
            return _places.ContainsKey(plusCode);
        }

        public bool TryGetValue(string plusCode, out PointT place)
        {
            return _places.TryGetValue(plusCode, out place);
        }

        public PointT? FindNextNewPlace(PointT current)
        {
            if (_places.Count == 0) return null;
            return _places.Values
                    .Where(p => p.Type == PinDesignFactory.New.TypeCh)
                    .Where(p => p.PlusCode != current.PlusCode)
                    .OrderBy(p => p.DistanceTo(current))
                    .FirstOrDefault();
        }

        public PointT FindPlace(string plusCode)
        {
            _places.TryGetValue(plusCode, out var place);
            return place;
        }
        
        public void Update(PointT place)
        {
            if (PlusCodeHelper.IsValid(place.PlusCode))
            {
                _places[place.PlusCode] = place;
            }
            else 
                throw new ApplicationException("Unrecognized plus code - " + place.PlusCode);
        }

        public void Downsize()
        {
            foreach (var p in Places.ToArray())
            {
                if (p.Type == PlaceStates.Ignored || p.Type == PlaceStates.PlaceNotAPlace)
                {
                    _places.Remove(p.PlusCode);
                }
            }
        }
    }
}
