using System;
using System.Threading.Tasks;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.Maui;

namespace CFAN.SchoolMap.MVVM
{
    public interface IViewManager
    {
        void Init(AppShell shell);
        Type GetViewModelType(IView view);

        Task Navigate<TView>(Param[] parameters = null, string prefix =null)
            where TView : IView;
        Task Navigate<TView>(params Param[] parameters) where TView : IView;
        void CloseCurrentView(Param[] returnParameters = null);
    }
}