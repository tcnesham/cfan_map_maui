using CFAN.SchoolMap.Maui.Database;
using CFAN.SchoolMap.Helpers;
using Microsoft.Maui.Controls;
using Microsoft.Maui.ApplicationModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;
using CFAN.SchoolMap.Maui.Services.Auth;

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
		
		// Subscribe to authentication and role change messages
		WeakReferenceMessenger.Default.Register<CurrentUserChangedMessage>(this, (r, m) => RefreshItemVisibility());
		ViewModels.Messaging.Subscribe(this, ViewModels.Message.MyRolesChanged, RefreshItemVisibility);
		ViewModels.Messaging.Subscribe(this, ViewModels.Message.RolesChanged, RefreshItemVisibility);
		
		// Initialize asynchronously to avoid blocking the constructor
		_ = InitializeAsync();
	}
	
	private async Task InitializeAsync()
	{
		// Initialize Repository property safely
		await Task.Delay(100); // Small delay to allow services to initialize
		
		try
		{
			System.Diagnostics.Debug.WriteLine($"ServiceHelper.Services is null: {ServiceHelper.Services == null}");
			
			_repository = ServiceHelper.GetService<IRepository>();
			if (_repository == null)
			{
				System.Diagnostics.Debug.WriteLine("First attempt to get Repository failed, trying alternative approach");
				// Try alternative approach
				_repository = ServiceHelper.Services?.GetService<IRepository>();
			}
			
			if (_repository == null)
			{
				System.Diagnostics.Debug.WriteLine("Repository is still null after both attempts");
			}
			else
			{
				System.Diagnostics.Debug.WriteLine($"Repository resolved successfully: {_repository.GetType().Name}");
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
			
			// Debug logging
			System.Diagnostics.Debug.WriteLine($"UpdateItemVisibility - School: {hasSchoolRoles}, Market: {hasMarketRoles}, Admin: {hasAdministrationRoles}");
			System.Diagnostics.Debug.WriteLine($"Repository: {_repository != null}, CurrentUser: {_repository?.CurrentUser?.Email}");
			
			if (_schoolMapItem != null)
			{
				_schoolMapItem.IsVisible = hasSchoolRoles;
				if (_schoolMapItem.Items.Count > 0)
					_schoolMapItem.Items[0].IsVisible = hasSchoolRoles;
				System.Diagnostics.Debug.WriteLine($"School Map Item IsVisible: {_schoolMapItem.IsVisible}");
			}
			
			if (_schoolStatsItem != null)
			{
				_schoolStatsItem.IsVisible = hasSchoolRoles;
				if (_schoolStatsItem.Items.Count > 0)
					_schoolStatsItem.Items[0].IsVisible = hasSchoolRoles;
				System.Diagnostics.Debug.WriteLine($"School Stats Item IsVisible: {_schoolStatsItem.IsVisible}");
			}
			
			if (_outreachMapItem != null)
			{
				_outreachMapItem.IsVisible = hasMarketRoles;
				if (_outreachMapItem.Items.Count > 0)
					_outreachMapItem.Items[0].IsVisible = hasMarketRoles;
				System.Diagnostics.Debug.WriteLine($"Outreach Map Item IsVisible: {_outreachMapItem.IsVisible}");
			}
			
			if (_administrationItem != null)
			{
				_administrationItem.IsVisible = hasAdministrationRoles;
				if (_administrationItem.Items.Count > 0)
					_administrationItem.Items[0].IsVisible = hasAdministrationRoles;
				System.Diagnostics.Debug.WriteLine($"Administration Item IsVisible: {_administrationItem.IsVisible}");
			}
			
			// Also trigger property change notifications for any binding that might still exist
			OnPropertyChanged(nameof(HasSchoolRoles));
			OnPropertyChanged(nameof(HasMarketRoles));
			OnPropertyChanged(nameof(HasAdministrationRoles));
		}
		catch (Exception ex)
		{
			System.Diagnostics.Debug.WriteLine($"Failed to update item visibility: {ex.Message}");
		}
	}
	
	// Public method to refresh visibility from external calls (e.g., after login)
	public void RefreshItemVisibility()
	{
		System.Diagnostics.Debug.WriteLine("RefreshItemVisibility called");
		MainThread.BeginInvokeOnMainThread(async () =>
		{
			// Force refresh the current user data from repository
			if (_repository != null)
			{
				try
				{
					var currentUser = await _repository.GetCurrentUser();
					System.Diagnostics.Debug.WriteLine($"Refreshed current user: {currentUser?.Email}");
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Failed to refresh current user: {ex.Message}");
				}
			}
			
			UpdateItemVisibility();
		});
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
