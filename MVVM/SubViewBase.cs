using System;
using System.Diagnostics;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CFAN.SchoolMap.MVVM
{
    public class SubViewBase : Frame, IView
    {
        private IViewModel _viewModel;

        public IViewModel ViewModel
        {
            get => _viewModel;
            set
            {
                _viewModel = value;
                BindingContext = _viewModel;
            }
        }

        public SubViewBase()
        {
            Debug.WriteLine($"Loading view {GetType().Name}");
            var vManager = DependencyService.Get<IViewManager>();
            var vmt = vManager.GetViewModelType(this);
            ViewModel = Activator.CreateInstance(vmt) as IViewModel;
            Debug.WriteLine($"Viewmodel {vmt.Name} loaded");
        }
    }
}
