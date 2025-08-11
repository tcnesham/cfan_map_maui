using CFAN.SchoolMap.Enumerations;
using CFAN.SchoolMap.Maui.Model;
using CFAN.SchoolMap.Model;

namespace CFAN.SchoolMap.Pins.States
{
    public static class PlaceStates
    {
        public const char SearchResult = '?';//place state
        public const char NotSaved = 'N';//place state
        public const char Unvisited = 'U';//place state
        public const char PlaceWithNote = 'u';//place state
        public const char WithOldVisit = 'H';//place state

        public const char PlacePlanned = 'P'; //place and visit state
        public const char VisitLater = 'L';//place and visit state
        public const char NoAccess = 'O'; //place and visit state
        
        //nepořádek
        public const char PlaceVisitAllowed = 'A';//visit state (V in places)
        public const char Visited = 'V';//place state (A in visits)
        public const char PlaceNotAPlace = 'X';//visit state (I in places)
        public const char Ignored = 'I';//place state (X in visits)

        // Markets
        public const char DontGoMarket = 'B';
        public const char ReadyMarket = 'b';
        public const char DoneMarket = 'm';
        public const char DoAgainMarket = 'M';
        // Fields
        public const char DontGoField = 'D';
        public const char ReadyField = 'd';
        public const char DoneField = 'f';
        public const char DoAgainField = 'F';
        // Misc
        public const char UnknownState = 'n';

        public static PinDesign GetTypeByState(SchoolVisit visit)
        {
            if (visit.IsUnvisited)
            {
                return string.IsNullOrWhiteSpace(visit.Note)
                    ? PinDesignFactory.Unvisited
                    : PinDesignFactory.UnvisitedWithNote;
            }

            if (visit.IsVisitLater) return PinDesignFactory.VisitLater;
            if (visit.IsNotASchool) return PinDesignFactory.Ignored;
            if (visit.IsNoAccess) return PinDesignFactory.NoAccess;
            return PinDesignFactory.Visited;
        }

        public static PinDesign GetTypeByState(MarketInfo marketInfo)
        {
            if (marketInfo == null)
            {
                return PinDesignFactory.UnknownMarket;
            }
            if (marketInfo.IsOpenField)
            {
                return marketInfo.Quality switch
                {
                    MarketQuality.DontGo => PinDesignFactory.DontGoField,
                    MarketQuality.Ready => PinDesignFactory.ReadyField,
                    MarketQuality.Done => PinDesignFactory.DoneField,
                    MarketQuality.DoAgain => PinDesignFactory.DoAgainField,
                    _ => PinDesignFactory.UnknownMarket
                };
            }
            return marketInfo.Quality switch
            {
                MarketQuality.DontGo => PinDesignFactory.DontGoMarket,
                MarketQuality.Ready => PinDesignFactory.ReadyMarket,
                MarketQuality.Done => PinDesignFactory.DoneMarket,
                MarketQuality.DoAgain => PinDesignFactory.DoAgainMarket,
                _ => PinDesignFactory.UnknownMarket
            };
        }
    }
}
