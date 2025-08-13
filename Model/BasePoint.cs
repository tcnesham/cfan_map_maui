using CFAN.SchoolMap.Pins.States;
using CFAN.SchoolMap.Services.PlusCodes;
using Maui.GoogleMaps;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CFAN.SchoolMap.Maui.Model
{
    public abstract class BasePoint:INotifyPropertyChanged
    {

        public const string PlaceNamePlaceholder = "...";
        private Position _position;

        private bool _positionLoaded = false;
        private string? _name;

        public string Country { get; set; } = string.Empty;

        [Ignored]
        public bool HasName => Name != null;
        public string? ChangedBy { get; set; }

        public string? Name
        {
            get => _name;
            set
            {
                if (value == _name) return;
                _name = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasName));
            }
        }

        public string PlusCode { get; set; }
        [Ignored]
        public Position Position
        {
            get
            {
                if (!_positionLoaded)
                {
                    _position = PlusCodeHelper.ToPosition(PlusCode);
                    _positionLoaded = true;
                }
                return _position;
            }
        }
        public byte Team { get; set; }

        [Ignored] public char TeamChar
        {
            get => (Team < 10)
                ? (char)('0' + Team)
                : (char)('A' + Team - 10);
            set
            {
                if (value >= '0' && value <= '9') Team = (byte) (value - '0');
                else if (value >= 'A' && value <= 'Z') Team = (byte)(value - 'A' + 10);
                else Team = 0;
            }
        }
        public char Type { get; set; }

        [ServerTimestamp]
        public Timestamp UpdatedAt { get; set; }
        public BasePoint(string plusCode, char type)
        {
            PlusCode = plusCode;
            Type = type;
            _position = new Position(0, 0);
        }

        public BasePoint() 
        { 
            _position = new Position(0, 0);
        }
        public string CreateBackupKey()
        {
            return Country + "_" + DateTime.Now.ToString("yyMMddhhmmss") + "_" + PlusCode;
        }

        public double DistanceTo(BasePoint o)
        {
            var dla = Math.Abs(Position.Latitude - o.Position.Latitude)*100;
            var dlo = Math.Abs(Position.Longitude - o.Position.Longitude)*100;
            return Math.Sqrt(dla * dla + dlo * dlo);
        }
        public string GetPinName()
        {
            return Name ?? PlaceNamePlaceholder;
        }

        [Ignored]
        public bool IsPlanned => Type == PlaceStates.PlacePlanned;

        public abstract string GetUnknownPlaceName();
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
