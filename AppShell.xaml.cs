namespace CFAN.SchoolMap.Maui;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();
		
		// Ensure we start on the Home page
		CurrentItem = Items[0]; // First tab (Home)
	}
}
