using System;
namespace CFAN.SchoolMap.Maui.GoogleMaps.Internals
{
    public interface IAnimationCallback
    {
        void OnFinished();
        void OnCanceled();
    }
}
