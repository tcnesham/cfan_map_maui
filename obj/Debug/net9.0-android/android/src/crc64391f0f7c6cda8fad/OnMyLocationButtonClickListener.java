package crc64391f0f7c6cda8fad;


public class OnMyLocationButtonClickListener
	extends java.lang.Object
	implements
		mono.android.IGCUserPeer,
		com.google.android.gms.maps.GoogleMap.OnMyLocationButtonClickListener
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_onMyLocationButtonClick:()Z:GetOnMyLocationButtonClickHandler:Android.Gms.Maps.GoogleMap/IOnMyLocationButtonClickListenerInvoker, Xamarin.GooglePlayServices.Maps\n" +
			"";
		mono.android.Runtime.register ("Maui.GoogleMaps.Platforms.Android.Listeners.OnMyLocationButtonClickListener, Maui.GoogleMaps", OnMyLocationButtonClickListener.class, __md_methods);
	}

	public OnMyLocationButtonClickListener ()
	{
		super ();
		if (getClass () == OnMyLocationButtonClickListener.class) {
			mono.android.TypeManager.Activate ("Maui.GoogleMaps.Platforms.Android.Listeners.OnMyLocationButtonClickListener, Maui.GoogleMaps", "", this, new java.lang.Object[] {  });
		}
	}

	public boolean onMyLocationButtonClick ()
	{
		return n_onMyLocationButtonClick ();
	}

	private native boolean n_onMyLocationButtonClick ();

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}
