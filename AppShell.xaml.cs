using CFAN.SchoolMap.Maui.Database;
using CFAN.SchoolMap.Helpers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CFAN.SchoolMap.Maui;

public partial class AppShell : Shell, INotifyPropertyChanged
{
	private IRepository? _repository;
	private FlyoutItem? _schoolMapItem;
	private FlyoutItem? _schoolStatsItem;
	private FlyoutItem? _outreachMapItem;
	private FlyoutItem? _administrationItem;
	
	public AppShell()
	{
		InitializeComponent();
		
		// Get references to the FlyoutItems
		_schoolMapItem = Items.OfType<FlyoutItem>().FirstOrDefault(x => x.Title == "School Map");
		_schoolStatsItem = Items.OfType<FlyoutItem>().FirstOrDefault(x => x.Title == "School Statistics");
		_outreachMapItem = Items.OfType<FlyoutItem>().FirstOrDefault(x => x.Title == "Outreach Map");
		_administrationItem = Items.OfType<FlyoutItem>().FirstOrDefault(x => x.Title == "Administration");
		
		// Initialize asynchronously to avoid blocking the constructor
		_ = InitializeAsync();
	}
	
	private async Task InitializeAsync()
	{
		// Initialize Repository property safely
		await Task.Delay(100); // Small delay to allow services to initialize
		
		try
		{
			_repository = ServiceHelper.GetService<IRepository>();
			if (_repository == null)
			{
				// Try alternative approach
				_repository = ServiceHelper.Services?.GetService<IRepository>();
			}
			
			// Subscribe to repository property changes if available
			if (_repository != null)
			{
				_repository.PropertyChanged += OnRepositoryPropertyChanged;
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to resolve Repository service: {ex.Message}");
		}
		
		// Set binding context and update visibility on main thread
		await MainThread.InvokeOnMainThreadAsync(() =>
		{
			try
			{
				BindingContext = this;
				
				// Update visibility based on current roles
				UpdateItemVisibility();
				
				// Ensure we start on the About page safely
				if (Items.Count > 0)
				{
					CurrentItem = Items[0]; // First tab (About)
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Failed to set binding context or current item: {ex.Message}");
			}
		});
	}
	
	private void UpdateItemVisibility()
	{
		try
		{
			bool hasSchoolRoles = _repository?.HasSchoolRoles ?? false;
			bool hasMarketRoles = _repository?.HasMarketRoles ?? false;
			bool hasAdministrationRoles = _repository?.HasAdministrationRoles ?? false;
			
			if (_schoolMapItem != null)
			{
				_schoolMapItem.IsVisible = hasSchoolRoles;
				if (_schoolMapItem.Items.Count > 0)
					_schoolMapItem.Items[0].IsVisible = hasSchoolRoles;
			}
			
			if (_schoolStatsItem != null)
			{
				_schoolStatsItem.IsVisible = hasSchoolRoles;
				if (_schoolStatsItem.Items.Count > 0)
					_schoolStatsItem.Items[0].IsVisible = hasSchoolRoles;
			}
			
			if (_outreachMapItem != null)
			{
				_outreachMapItem.IsVisible = hasMarketRoles;
				if (_outreachMapItem.Items.Count > 0)
					_outreachMapItem.Items[0].IsVisible = hasMarketRoles;
			}
			
			if (_administrationItem != null)
			{
				_administrationItem.IsVisible = hasAdministrationRoles;
				if (_administrationItem.Items.Count > 0)
					_administrationItem.Items[0].IsVisible = hasAdministrationRoles;
			}
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to update item visibility: {ex.Message}");
		}
	}
	
	private void OnRepositoryPropertyChanged(object? sender, PropertyChangedEventArgs e)
	{
		// Forward repository property changes to update UI bindings
		if (e.PropertyName == nameof(IRepository.HasSchoolRoles) ||
		    e.PropertyName == nameof(IRepository.HasMarketRoles) ||
		    e.PropertyName == nameof(IRepository.HasAdministrationRoles))
		{
			MainThread.BeginInvokeOnMainThread(UpdateItemVisibility);
		}
	}
	
	public IRepository? Repository => _repository;
	
	// Safe wrapper properties for XAML binding
	public bool HasSchoolRoles => _repository?.HasSchoolRoles ?? false;
	public bool HasMarketRoles => _repository?.HasMarketRoles ?? false;
	public bool HasAdministrationRoles => _repository?.HasAdministrationRoles ?? false;
	
	public Command NavigateToHelpCommand => new Command(async () =>
	{
		try
		{
			// Navigate to help/resources page or open URL
			await Launcher.OpenAsync("https://example.com/help"); // Replace with your actual help URL
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to open help URL: {ex.Message}");
		}
	});
	
	public new event PropertyChangedEventHandler? PropertyChanged;
	
	protected new virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
