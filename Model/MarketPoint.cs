using System;
// using CFAN.SchoolMap.Enumerations;
// using CFAN.SchoolMap.Pins;
// using CFAN.SchoolMap.Pins.States;
// using CFAN.SchoolMap.Services.PlusCodes;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using Microsoft.Maui.Controls.Maps;

namespace CFAN.SchoolMap.Maui.Model
{
    public class MarketPoint : BasePoint
    {
        public override string GetUnknownPlaceName() => "Unknown place";

        public MarketPoint(string plusCode, char type) : base(plusCode, type) { }

        public MarketPoint() : base() { }
    }
}
