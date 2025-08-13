using System.Windows.Input;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Services.Auth;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.MVVM;
#if IOS || MACCATALYST
using Foundation;
#endif

namespace CFAN.SchoolMap.ViewModels
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public class AboutViewModel : BaseVM
    {
        public Command LoginCommand { get; }
        public Command LogoutCommand { get; }
        public string Email { get; set; }
        public string Password { get; set; } = string.Empty;
        public ICommand OpenWebCommand { get; }
        public Command ResetPwdCommand { get; }

        public string Version => "Version:" + VersionTracking.CurrentVersion;

        public AboutViewModel() 
        {
            Title = "About CfaN RED";
            OpenWebCommand = new SafeCommand(async () => await Browser.OpenAsync("https://www.cfan.eu/about-us/vision"));
            LoginCommand = new SafeCommand(Login);
            LogoutCommand = new SafeCommand(Logout);
            ResetPwdCommand = new SafeCommand(ResetPwd);
            
            // Safely handle the case where Auth or User might be null
            Email = Auth?.User?.Email ?? "";
        }
        private async Task ResetPwd()
        {
            if (Email.IsNullOrWhiteSpace())
            {
                Dialogs.Toast("Fill in your email first!");
            }
            else if (await Dialogs.ConfirmAsync("Do you want to send reset password email? Link is valid 30 minutes."))
            {
                await Auth.TryResetPwd(Email);
            }
        }
        private async Task Login(object obj)
        {
            string token = await Auth.LoginWithEmailPassword(Email, Password);
            if (token == "")
            {
                Dialogs.Toast("Login failed!");
                return;
            }

            await SecureStorage.SetAsync("UserEmail", Email);
            await SecureStorage.SetAsync("UserPassword", Password);
        }

        private void Logout(object obj)
        {
            Dialogs.Toast(Auth.LogOut() ? "You are logged out!" : "Log out failed!");
            Email = "";
            Notify(nameof(Email));
            Password = "";
            Notify(nameof(Password));
        }
    }
}
