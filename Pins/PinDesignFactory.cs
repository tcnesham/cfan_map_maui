using System;
using System.Collections.Generic;
using CFAN.SchoolMap.Maui.Model;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.Pins.States;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Graphics;

namespace CFAN.SchoolMap.Pins
{
    public static class PinDesignFactory
    {
        private static readonly Dictionary<string, PinDesign> ByCode = new();

        static PinDesignFactory()
        {
            AddDesign(Ignored);
            AddDesign(New);
            AddDesign(Unvisited);
            AddDesign(UnvisitedWithNote);
            AddDesign(Visited);
            AddDesign(VisitLater);
            AddDesign(NoAccess);
            AddDesign(WithOldVisit);

            AddDesign(UnknownMarket);
            AddDesign(ReservedPlace);
            AddDesign(DontGoField);
            AddDesign(DontGoMarket);
            AddDesign(ReadyField);
            AddDesign(ReadyMarket);
            AddDesign(DoneField);
            AddDesign(DoneMarket);
            AddDesign(DoAgainField);
            AddDesign(DoAgainMarket);

            for (char c = 'A'; c <= 'Z'; c++) AddDesign(new PinDesign(PlaceStates.PlacePlanned.ToString() + c, "t" + c, true));
            for (char c = '0'; c <= '9'; c++) AddDesign(new PinDesign(PlaceStates.PlacePlanned.ToString() + c, "t" + c, true));
        }

        public static PinDesign Get(BasePoint place)
        {
            return place.Type == PlaceStates.PlacePlanned
                ? GetByTeam(place.TeamChar) 
                : GetByType(place.Type.ToString());
        }

        public static PinDesign GetByTeam(char teamChar)
        {
            return GetByType(PlaceStates.PlacePlanned.ToString() + teamChar);
        }

        public static PinDesign GetByType(char type, char teamChar)
        {
            return type == PlaceStates.PlacePlanned
                ? GetByTeam(teamChar) : 
                GetByType(type.ToString());
        }

        public static PinDesign GetByType(string type)
        {
            ByCode.TryGetValue(type, out var c);
            if (c == null)
            {
                throw new ApplicationException("Unknown pin design for " + c);
            }
            return c;
        }

        private static void AddDesign(PinDesign design)
        {
            ByCode.Add(design.Type, design);
        }

        private static Color green = Color.FromHsv(125, 100, 100);
        private static Color oldGreen = Color.FromHsv(165, 100, 100);
        private static Color yellow = Color.FromHsv(50, 100, 100);
        private static Color red = Colors.Red;
        private static Color purple = Colors.DarkViolet;
        private static Color blue = Colors.Blue;
        private static Color orange = Color.FromRgb(0xf9, 0x73, 0x16);

        public static PinDesign Ignored { get; } = new(PlaceStates.Ignored, Colors.White);
        public static PinDesign NoAccess { get; } = new(PlaceStates.NoAccess.ToString(), "black");
        public static PinDesign New { get; } = new(PlaceStates.NotSaved, yellow);
        public static PinDesign WithOldVisit { get; } = new(PlaceStates.WithOldVisit, oldGreen);
        public static PinDesign SearchResult { get; } = new(PlaceStates.SearchResult, yellow);
        public static PinDesign Unvisited { get; } = new(PlaceStates.Unvisited, green);//green
        public static PinDesign UnvisitedWithNote { get; } = new(PlaceStates.PlaceWithNote, green);
        public static PinDesign Visited { get; } = new(PlaceStates.Visited, red);
        public static PinDesign VisitLater { get; } = new(PlaceStates.VisitLater, purple);

        // Markets Pin designs
        // Misc
        public static PinDesign UnknownMarket { get; } = new(PlaceStates.UnknownState, green);
        public static PinDesign ReservedPlace { get; } = new(PlaceStates.PlacePlanned, blue);

        // Markets
        public static PinDesign DontGoMarket { get; } = new(PlaceStates.DontGoMarket.ToString(), "black");
        public static PinDesign ReadyMarket { get; } = new(PlaceStates.ReadyMarket, green); 
        public static PinDesign DoneMarket { get; } = new(PlaceStates.DoneMarket, red);
        public static PinDesign DoAgainMarket { get; } = new(PlaceStates.DoAgainMarket, purple);

        // Fields
        public static PinDesign DontGoField { get; } = new(PlaceStates.DontGoField.ToString(), "blackfield");
        public static PinDesign ReadyField { get; } = new(PlaceStates.ReadyField.ToString(), "greenfield"); 
        public static PinDesign DoneField { get; } = new(PlaceStates.DoneField.ToString(), "redfield"); 
        public static PinDesign DoAgainField { get; } = new(PlaceStates.DoAgainField.ToString(), "purplefield");

    }
}
