using System;
using System.IO;

namespace CFAN.SchoolMap.Maui.GoogleMaps.Internals
{
    public sealed class TakeSnapshotMessage
    {
        public Action<Stream> OnSnapshot { get; }

        public TakeSnapshotMessage(Action<Stream> onSnapshot)
        {
            OnSnapshot = onSnapshot;
        }
    }
}