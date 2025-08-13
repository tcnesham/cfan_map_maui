using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Acr.UserDialogs;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Database;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.Services.Auth;
using CFAN.SchoolMap.Services.Exceptions;
using static System.String;
using User = CFAN.SchoolMap.Model.User;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
#if IOS || MACCATALYST
using Foundation;
#endif
using CFAN.SchoolMap.Maui.Database;

namespace CFAN.SchoolMap.MVVM
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public class BaseVM : INotifyPropertyChanged, IViewModel, IQueryAttributable
    {
        private bool _isRefreshing;
        public Task Initialization { get; set; }
        public Task LoadingData { get; set; }
        protected virtual bool UsesRepository => true;

        public BaseVM()
        {
            if (UsesRepository)
            {
                Repository = DependencyService.Resolve<IRepository>();
                Initialization = Init();
            }

            RefreshCmd = new SafeCommand(Refresh);
        }

        public Command RefreshCmd { get; }

        public bool IsRefreshing
        {
            get => _isRefreshing;
            set
            {
                _isRefreshing = value;
                Notify(nameof(IsRefreshing));
            }
        }
        
        public void ApplyQueryAttributes(IDictionary<string, string> query)
        {
            var pars = Param.DecodeValues(query);
            LoadingData = LoadData(pars);
        }

        protected virtual async Task LoadData(Dictionary<string, string> pars)
        {
            await OnLoadData(pars);
            Refresh();
        }

        protected async Task Init()
        {
            CurrentUser = await Repository?.GetCurrentUser();
            Refresh();
        }

        public User CurrentUser
        {
            get => _currentUser;
            set
            {
                _currentUser = value;
                RefreshRoles();
            }
        }

        protected virtual void RefreshRoles() { }

        protected virtual void Refresh() { }

        protected virtual void PermissionChanged()
        {
            Refresh();
        }

        protected async virtual Task OnLoadData(Dictionary<string, string> pars) { }

        public IRepository Repository { get; set; }
        public IUserDialogs Dialogs { get; } = UserDialogs.Instance;
        public IExceptionHandler ExceptionHandler { get; } = DependencyService.Resolve<IExceptionHandler>();

        public IViewManager ViewManager { get; } = DependencyService.Resolve<IViewManager>();
        public IAuth Auth { get; } = DependencyService.Resolve<IAuth>();

        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        string _title = Empty;
        private bool _changed;
        private User _currentUser;

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        protected void ChangeProperty<T>(ref T backingStore, T value, [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            Changed = SetProperty(ref backingStore, value, propertyName, onChanged) || _changed;
            Notify(propertyName);
        }

        public bool Changed
        {
            get => _changed;
            set => _changed = value;
        }

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            Notify(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void Notify([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void NotifyAll()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(Empty));
        }

        protected bool HasRole(Role role)
        {
            return Auth.IsAdmin || Auth.IsTester || (CurrentUser?.Roles?.Contains(role) ?? false);
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            throw new NotImplementedException();
        }
    }
}
