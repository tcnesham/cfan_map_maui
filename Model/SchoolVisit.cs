using System.ComponentModel;
using System.Runtime.CompilerServices;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Maui;
using CFAN.SchoolMap.Pins.States;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;

namespace CFAN.SchoolMap.Maui.Model
{
    public class SchoolVisit: INotifyPropertyChanged
    {
        private int _nOfChildren = 500;
        private int _percOfConverts = 70;
        private char _state = PlaceStates.PlaceVisitAllowed;

        public SchoolVisit() { }
        public SchoolVisit(string plusCode)
        {
            PlusCode = plusCode;
        }

        public string PlusCode { get; set; } = "";

        public string Country { get; set; } = "";

        public DateTime Date { get; set; } = DateTime.Now;

        public string ChangedBy { get; set; } = "";

        public string Note { get; set; } = "";

        [ServerTimestamp]
        public Timestamp UpdatedAt { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void Notify([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int NOfChildren
        {
            get => _nOfChildren;
            set
            {
                _nOfChildren = value;
                Notify(nameof(NOfChildren));
            }
        }

        public int PercOfConverts
        {
            get => _percOfConverts;
            set
            {
                _percOfConverts = Math.Min(Math.Max(value,0),100);
                Notify(nameof(PercOfConverts));
            }
        }

        public char State
        {
            get => _state;
            set
            {
                _state = value;
                Notify(nameof(State));
                Notify(nameof(IsUnvisited));
                Notify(nameof(IsAllowed));
                Notify(nameof(IsVisitLater));
                Notify(nameof(IsNotASchool));
                Notify(nameof(IsNoAccess));

            }
        }
        [Ignored]
        public bool IsAllowed
        {
            get => State == PlaceStates.PlaceVisitAllowed;
            set
            {
                if (value) State = PlaceStates.PlaceVisitAllowed;
            }
        }
        [Ignored]
        public bool IsUnvisited
        {
            get => State == PlaceStates.Unvisited;
            set
            {
                if (value) State = PlaceStates.Unvisited;
            }
        }
        [Ignored]
        public bool IsVisitLater
        {
            get => State == PlaceStates.VisitLater;
            set
            {
                if (value) State = PlaceStates.VisitLater;
            }
        }

        [Ignored]
        public bool IsNotASchool
        {
            get => State == PlaceStates.PlaceNotAPlace;
            set
            {
                if (value) State = PlaceStates.PlaceNotAPlace;
            }
        }

        [Ignored]
        public bool IsNoAccess
        {
            get => State == PlaceStates.NoAccess;
            set
            {
                if (value) State = PlaceStates.NoAccess;
            }
        }

        [Ignored]
        public string UpdatedVisualization => $"Last updated by {ChangedBy} ({UpdatedAt.ToDateTime()})";

        [Ignored]
        public SafeCommand MorePopulationCommand => new(() => 
        {
            NOfChildren += 100;
        });

        [Ignored]
        public SafeCommand LessPopulationCommand => new(() =>
        {
            NOfChildren = Math.Max(NOfChildren-100,0);
        });

        [Ignored]
        public SafeCommand MoreConvertsCommand => new(() =>
        {
            PercOfConverts = Math.Min(PercOfConverts + 5, 100);
        });

        [Ignored]
        public SafeCommand LessConvertsCommand => new(() =>
        {
            PercOfConverts = Math.Max(PercOfConverts - 5, 0);
        });
        public string CreateBackupKey()
        {
            return PlusCode + "_" + Date.Year;
        }
    }
}
