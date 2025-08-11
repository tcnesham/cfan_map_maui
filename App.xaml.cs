namespace CFAN.SchoolMap.Maui;

public partial class App : Application
{
	public const string Key = "AIzaSyAZqhVZWwYQBkCCti5K4v0EtSLQjfdNGOY";
	public App()
	{
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}
}