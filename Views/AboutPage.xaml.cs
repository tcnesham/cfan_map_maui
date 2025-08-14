using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using CFAN.SchoolMap.ViewModels;

namespace CFAN.SchoolMap.Maui
{
    public partial class AboutPage : ContentPage
    {
        private AboutViewModel? _viewModel;

        public AboutPage()
        {
            InitializeComponent();
            
            // Initialize asynchronously to avoid startup crashes
            _ = InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            try
            {
                // Small delay to ensure services are ready
                await Task.Delay(10);
                
                _viewModel = new AboutViewModel();
                BindingContext = _viewModel;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error creating AboutViewModel: {ex}");
                
                // Create a minimal fallback if needed
                BindingContext = new
                {
                    Title = "About CfaN RED",
                    Version = "Version: Unknown",
                    Auth = (object?)null,
                    Email = "",
                    Password = ""
                };
            }
        }
    }
}