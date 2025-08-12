namespace CFAN.SchoolMap.Maui;


public partial class MainPage : ContentPage
{
	int count = 0;

	public MainPage()
	{
		InitializeComponent();
	}


	private void OnCounterClicked(object? sender, EventArgs e)
	{
		count++;

		if (count == 1)
			CounterBtn.Text = $"Clicked {count} time";
		else
			CounterBtn.Text = $"Clicked {count} times";

		SemanticScreenReader.Announce(CounterBtn.Text);
	}

	private void OnPlusCodeButtonClicked(object? sender, EventArgs e)
	{
		String placeIdString = PlaceIdEntry.Text;
		if (string.IsNullOrWhiteSpace(placeIdString))
		{
			DisplayAlert("Error", "Please enter a valid Plus Code.", "OK");
			return;
		}
		getPlace(placeIdString);
	}

	private async Task getPlace(String placeIdString)
	{
		var placeService = new CFAN.SchoolMap.Services.Places.PlaceService();
		var result = await placeService.GetPluscodeOfPlace(placeIdString);
		await DisplayAlert("Ok", "Result " + result, "OK");
	}
}
