using System.IO;

namespace CFAN.SchoolMap.Maui.GoogleMaps
{
    public static class BitmapDescriptorFactory
    {
        public static BitmapDescriptor DefaultMarker(Color color)
        {
            return BitmapDescriptor.DefaultMarker(color, $"DefaultMarker_{color}");
        }

        public static BitmapDescriptor FromBundle(string bundleName)
        {
            return BitmapDescriptor.FromBundle(bundleName, $"Bundle_{bundleName}");
        }

        public static BitmapDescriptor FromStream(Stream stream)
        {
            return BitmapDescriptor.FromStream(stream, $"Stream_{stream.GetHashCode()}");
        }

        public static BitmapDescriptor FromPath(string absolutePath)
        {
            return BitmapDescriptor.FromPath(absolutePath, $"Path_{absolutePath}");
        }

        public static BitmapDescriptor FromView(View view)
        {
            return BitmapDescriptor.FromView(view, $"View_{view.GetHashCode()}");
        }
    }
}
