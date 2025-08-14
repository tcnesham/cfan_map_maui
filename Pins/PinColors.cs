using CFAN.SchoolMap.Maui.GoogleMaps;

namespace CFAN.SchoolMap.Pins
{
    public class PinDesign
    {
        private string _type;
        private bool? _isNew;
        private bool? _isHidden;
        private readonly string _iconName;
        private byte _opacity;

        public PinDesign(char type, Color color, byte opacity=100)
        {
            _type = type.ToString();
            Color = color;
            TeamNumbered = false;
            _isNew = null;
            _isHidden = null;
            _iconName = null;
            _opacity = opacity;
        }
        public PinDesign(string type, string iconName, bool teamNumbered = false)
        {
            _type = type;
            Color = Microsoft.Maui.Graphics.Colors.Blue;
            _iconName = iconName;
            TeamNumbered = teamNumbered;
            _isNew = null;
            _isHidden = null;
            _opacity = 100;
        }

        public string Type
        {
            get => _type;
            set
            {
                _type = value;
                _isNew = null;
            }
        }

        public char TypeCh => Type[0];
        public Color Color { get; set; }
        public bool TeamNumbered { get; }
        public bool IsNew => _isNew ?? (_isNew = Type == PinDesignFactory.New.Type).Value;
        public Microsoft.Maui.Controls.Maps.PinType PinType => IsNew ? Microsoft.Maui.Controls.Maps.PinType.SearchResult : Microsoft.Maui.Controls.Maps.PinType.SavedPin;
        public float Transparency => 1.0f - ((IsNew ? 0.9f : 1.0f) * ((float)_opacity / 100));
        public bool IsHidden => _isHidden ?? (_isHidden = Type == PinDesignFactory.Ignored.Type).Value;

        public BitmapDescriptor GetIcon()
        {
            return _iconName != null 
                ? BitmapDescriptorFactory.FromBundle(_iconName) 
                : BitmapDescriptorFactory.DefaultMarker(Color);
        }
    }
}
