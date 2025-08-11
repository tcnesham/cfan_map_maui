using CFAN.SchoolMap.Enumerations;
using Plugin.CloudFirestore;
using Plugin.CloudFirestore.Attributes;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CFAN.SchoolMap.Maui.Model
{
    public class MarketInfo : INotifyPropertyChanged
    {
        public MarketInfo() { }
        public MarketInfo(string plusCode)
        {
            PlusCode = plusCode;
        }

        public string PlusCode { get; set; } = "";

        public string Country { get; set; } = "";

        public DateTime? LastVisit { get; set; } = null;

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

        public MarketQuality Quality { get; set; } = MarketQuality.Ready;

        public bool IsOpenField { get; set; } = false;

        [Ignored]
        public bool IsDontGo//black
        {
            get => Quality == MarketQuality.DontGo;
            set
            {
                if (value) Quality = MarketQuality.DontGo;
                Notify();
            }
        }

        [Ignored]
        public bool IsReady
        {
            get => Quality == MarketQuality.Ready;
            set
            {
                if (value)
                {
                    Quality = MarketQuality.Ready;
                    Notify();
                }
            }
        }

        [Ignored]
        public bool IsDone
        {
            get => Quality == MarketQuality.Done;
            set
            {
                if (value)
                {
                    Quality = MarketQuality.Done;
                    Notify();
                }
            }
        }

        [Ignored]
        public bool IsDoAgain
        {
            get => Quality == MarketQuality.DoAgain;
            set
            {
                if (value)
                {
                    Quality = MarketQuality.DoAgain;
                    Notify();
                }
            }
        }
    }
}
