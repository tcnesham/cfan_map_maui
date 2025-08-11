using System.ComponentModel;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Messaging;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.Services.Auth;

namespace CFAN.SchoolMap.Maui.Services.Auth
{
    public abstract class AuthBase : IAuth, INotifyPropertyChanged
    {
        public abstract bool IsSignIn { get; }
        public abstract FBUser User { get; }

        public void SetCurrentUserRoles(Role[] roles)
        {
            Roles = roles;
            Notify(nameof(IsTester));
            Notify(nameof(IsAdmin));
            Notify(nameof(Roles));
        }

        public Role[]? Roles { get; set; }

        public bool IsTester => User?.Email == "cfanschooltester@gmail.com" || (Roles?.Contains(Role.Tester) ?? true);
        public bool IsAdmin => User?.Email == "jan.folbrecht@gmail.com" || (Roles?.Contains(Role.Admin) ?? false);

        public abstract Task<string> LoginWithEmailPassword(string email, string password);
        public abstract bool LogOut();
        public abstract Task<string> TryCreateUser(string email, string pwd);
        public abstract Task TryResetPwd(string email);

        public event PropertyChangedEventHandler? PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void Notify([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void StateChanged()
        {
            Notify(nameof(IsSignIn));
            Notify(nameof(User));
            Notify(nameof(IsTester));
            Notify(nameof(IsAdmin));
            WeakReferenceMessenger.Default.Send(new CurrentUserChangedMessage(this));
        }
    }
}
