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
        public Command TestAuthCommand { get; }
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
            TestAuthCommand = new SafeCommand(TestAuth);
            
            // Debug the Auth service resolution
            System.Diagnostics.Debug.WriteLine($"AboutViewModel - Auth service: {Auth}");
            System.Diagnostics.Debug.WriteLine($"AboutViewModel - Auth service type: {Auth?.GetType().FullName}");
            
            // Safely handle the case where Auth or User might be null
            try
            {
                Email = Auth?.User?.Email ?? "";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error accessing Auth in AboutViewModel: {ex}");
                Email = "";
            }
            
            // Auto-trigger real user authentication 5 seconds after construction for testing
            Task.Run(async () =>
            {
                await Task.Delay(5000);
                System.Diagnostics.Debug.WriteLine("AboutViewModel: Auto-triggering real user authentication");
                await TestRealUser();
            });
        }
        private async Task ResetPwd()
        {
            if (Email.IsNullOrWhiteSpace())
            {
                Dialogs.Toast("Fill in your email first!");
            }
            else if (await Dialogs.ConfirmAsync("Do you want to send reset password email? Link is valid 30 minutes."))
            {
                if (Auth != null)
                {
                    await Auth.TryResetPwd(Email);
                }
                else
                {
                    Dialogs.Toast("Authentication service not available!");
                }
            }
        }
        private async Task Login(object obj)
        {
            if (Auth == null)
            {
                Dialogs.Toast("Authentication service not available!");
                return;
            }
            
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
            if (Auth != null)
            {
                Dialogs.Toast(Auth.LogOut() ? "You are logged out!" : "Log out failed!");
            }
            else
            {
                Dialogs.Toast("Authentication service not available!");
            }
            
            Email = "";
            Notify(nameof(Email));
            Password = "";
            Notify(nameof(Password));
        }

        private async Task TestAuth()
        {
            string msg = $"Auth service status:\n";
            msg += $"Auth null: {Auth == null}\n";
            msg += $"Auth type: {Auth?.GetType().FullName}\n";
            msg += $"ServiceHelper.Services null: {CFAN.SchoolMap.Helpers.ServiceHelper.Services == null}\n";
            
            if (CFAN.SchoolMap.Helpers.ServiceHelper.Services != null)
            {
                var directAuth = CFAN.SchoolMap.Helpers.ServiceHelper.Services.GetService<CFAN.SchoolMap.Services.Auth.IAuth>();
                msg += $"Direct DI resolution: {directAuth?.GetType().FullName ?? "null"}\n";
            }
            
            // Test the authentication flow with test credentials
            System.Diagnostics.Debug.WriteLine("TestAuth: Starting test authentication flow");
            if (Auth != null)
            {
                string testToken = await Auth.LoginWithEmailPassword("test@test.com", "test123");
                msg += $"\nTest Login Result: {(string.IsNullOrEmpty(testToken) ? "FAILED" : "SUCCESS")}\n";
                msg += $"Token: {testToken}\n";
                msg += $"IsSignIn after test: {Auth.IsSignIn}\n";
                msg += $"User email after test: {Auth.User?.Email ?? "null"}\n";
                
                System.Diagnostics.Debug.WriteLine($"TestAuth: Login result - Token: {testToken}, IsSignIn: {Auth.IsSignIn}");
            }
            
            await Dialogs.AlertAsync(msg, "Auth Debug", "OK");
        }

        private async Task TestRealUser()
        {
            // Test the authentication flow with real user credentials (administrator)
            System.Diagnostics.Debug.WriteLine("TestRealUser: Starting admin user authentication flow");
            if (Auth != null)
            {
                string realToken = await Auth.LoginWithEmailPassword("tim.nesham@gmail.com", "09*red!tn");
                System.Diagnostics.Debug.WriteLine($"TestRealUser: Admin login result - Token: {(string.IsNullOrEmpty(realToken) ? "FAILED" : "SUCCESS")}, IsSignIn: {Auth.IsSignIn}");
                System.Diagnostics.Debug.WriteLine($"TestRealUser: Admin user email: {Auth.User?.Email ?? "null"}");
                
                // Wait a moment for menu updates
                await Task.Delay(2000);
                
                // Now test logout and login as regular user (simulated)
                Auth.LogOut();
                System.Diagnostics.Debug.WriteLine("TestRealUser: Logged out admin, now testing regular user fallback");
                
                // Simulate a regular user login that would hit the same Firestore permission issue
                string regularToken = await Auth.LoginWithEmailPassword("regular.user@example.com", "password123");
                System.Diagnostics.Debug.WriteLine($"TestRealUser: Regular user login result - Token: {(string.IsNullOrEmpty(regularToken) ? "FAILED" : "SUCCESS")}, IsSignIn: {Auth.IsSignIn}");
                System.Diagnostics.Debug.WriteLine($"TestRealUser: Regular user email: {Auth.User?.Email ?? "null"}");
            }
        }
    }
}
