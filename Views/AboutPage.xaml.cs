using System;
using System.ComponentModel;
using Microsoft.Maui.Controls.Xaml;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using CFAN.SchoolMap.ViewModels;

namespace CFAN.SchoolMap.Maui
{
    public partial class AboutPage : ContentPage
    {
        public AboutPage()
        {
            InitializeComponent();
            
            // Set the BindingContext after the page is initialized
            try
            {
                BindingContext = new AboutViewModel();
            }
            catch (Exception ex)
            {
                // Log the exception to help with debugging
                System.Diagnostics.Debug.WriteLine($"Error creating AboutViewModel: {ex}");
                throw;
            }
        }
    }
}