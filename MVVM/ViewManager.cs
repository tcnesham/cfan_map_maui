using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CFAN.SchoolMap.Database;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.MVVM;
using CFAN.SchoolMap.Views;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using CFAN.SchoolMap.Maui;

[assembly: Dependency(typeof(ViewManager))]

namespace CFAN.SchoolMap.MVVM
{
    public class ViewManager : IViewManager
    {
        private AppShell _shell;
        private Dictionary<string, Type> _viewModelTypes;

        public void Init(AppShell shell)
        {
            _shell = shell;
            var viewModelType = typeof(IViewModel);
            _viewModelTypes = GetType().Assembly.GetTypes().Where(p => viewModelType.IsAssignableFrom(p))
                .ToDictionary(t => t.Name, t => t);

            Routing.RegisterRoute(nameof(SchoolMapPage) + "/" + nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(MarketMapPage) + "/" + nameof(SearchPage), typeof(SearchPage));
            Routing.RegisterRoute(nameof(AdminPage) + "/" + nameof(AddUserPage), typeof(AddUserPage));
            Routing.RegisterRoute(nameof(AdminPage) + "/" + nameof(RolesPage), typeof(RolesPage));
            Routing.RegisterRoute(nameof(AdminPage) + "/" + nameof(RolesPage) + "/" + nameof(UserNameRolesPage),
                typeof(UserNameRolesPage));
        }

        public Type GetViewModelType(IView view)
        {
            var name = view.GetType().Name;
            name = name.Substring(0, name.Length - "Page".Length);
            name += "VM";
            _viewModelTypes.TryGetValue(name, out var t);
            if (t == null) throw new ApplicationException("Neznámý VM " + name);
            return t;
        }

        public async Task Navigate<TView>(params Param[] parameters) where TView : IView
        {
            await Navigate<TView>(parameters, null);
        }

        public async Task Navigate<TView>(Param[] parameters = null, string prefix = null) where TView : IView
        {
            parameters ??= new[] { new Param("action", "empty") };
            await _shell.GoToAsync(
                new ShellNavigationState(
                    $"{prefix ?? string.Empty}{typeof(TView).Name}{EncodeParameters(parameters)}"));
        }

        private static string EncodeParameters(Param[] parameters)
        {
            var routeSb = new StringBuilder();
            if (parameters != null && parameters.Any())
            {
                routeSb.Append("?");
                bool first = true;
                foreach (var p in parameters)
                {
                    if (p.Value == null)
                    {
                        throw new ApplicationException(p.Name);
                    }

                    if (first)
                    {
                        first = false;
                    }
                    else
                    {
                        routeSb.Append("&");
                    }

                    routeSb.Append(p.Name);
                    routeSb.Append("=");
                    routeSb.Append(Uri.EscapeUriString(p.Value));
                }
            }

            return routeSb.ToString();
        }

        public void CloseCurrentView(Param[] returnParameters = null)
        {
            _shell.GoToAsync(new ShellNavigationState($"..{EncodeParameters(returnParameters)}"));
        }
    }
}
