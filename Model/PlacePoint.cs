using CFAN.SchoolMap.Maui.Model;
using CFAN.SchoolMap.Pins;
using CFAN.SchoolMap.Pins.States;
using Plugin.CloudFirestore.Attributes;

namespace CFAN.SchoolMap.Model
{
    public class PlacePoint : BasePoint
    {
        public override string GetUnknownPlaceName()=> "Unknown school";

        public PlacePoint(string plusCode, char type) : base(plusCode, type) { }

        public PlacePoint() { }

        [Ignored]
        public bool IsNotVisible => Type == PlaceStates.Ignored;

        [Ignored]
        public bool IsVisited => Type == PinDesignFactory.Visited.TypeCh || Type == PinDesignFactory.WithOldVisit.TypeCh;

        [Ignored]
        public bool IsSchool => (Type != PinDesignFactory.Ignored.TypeCh);

        [Ignored]
        public bool IsNoAccess => (Type == PinDesignFactory.NoAccess.TypeCh);
        
        [Ignored]
        public bool CanHaveNote => (Type == PinDesignFactory.Visited.TypeCh) || (Type == PinDesignFactory.UnvisitedWithNote.TypeCh) || (Type == PinDesignFactory.VisitLater.TypeCh) || (Type == PinDesignFactory.NoAccess.TypeCh);
    }
}
