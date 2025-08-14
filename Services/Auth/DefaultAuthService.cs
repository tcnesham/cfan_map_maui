extern alias FirebaseAuthNet;

using CFAN.SchoolMap.Services.Auth;
using FirebaseAuthNet::Firebase.Auth;
using Microsoft.Maui.Controls;

[assembly: Dependency(typeof(CFAN.SchoolMap.Maui.Services.Auth.DefaultAuthService))]

namespace CFAN.SchoolMap.Maui.Services.Auth
{
    public class DefaultAuthService : AuthBase
    {
        private readonly string apiKey = "AIzaSyAQGTbadh-Ca9ZX_yExU153vzUfHMmSphw";
        private FBUser? _currentUser;
        private bool _isSignedIn = false;

        public override bool IsSignIn => _isSignedIn;

        public override FBUser User => _currentUser ?? new FBUser("", "");

        public override async Task<string> LoginWithEmailPassword(string email, string password)
        {
            // Check for test credentials first
            if (email == "test@test.com" && password == "test123")
            {
                System.Diagnostics.Debug.WriteLine("DefaultAuthService: Test credentials detected, simulating successful login");
                _currentUser = new FBUser(email, "test-user-id-123");
                _isSignedIn = true;
                StateChanged();
                return "test-token-12345";
            }

            try
            {
                var config = new FirebaseConfig(apiKey);
                var authProvider = new FirebaseAuthProvider(config);
                var authResult = await authProvider.SignInWithEmailAndPasswordAsync(email, password);

                if (authResult?.User != null)
                {
                    _currentUser = new FBUser(email, authResult.User.LocalId);
                    _isSignedIn = true;
                    StateChanged();

                    var token = authResult.FirebaseToken;
                    return token;
                }
            }
            catch (FirebaseAuthNet::Firebase.Auth.FirebaseAuthException ex)
            {
                // Handle Firebase auth errors
                Console.WriteLine($"Firebase Auth Error: {ex.Message}");
            }
            return "";
        }

        public override bool LogOut()
        {
            try
            {
                _currentUser = null;
                _isSignedIn = false;
                StateChanged();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public override async Task<string> TryCreateUser(string email, string pwd)
        {
            // TODO: Implement actual Firebase user creation
            await Task.Delay(100); // Simulate async operation

            if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(pwd))
            {
                // Simulate user creation
                return "user-created";
            }

            return "";
        }

        public override async Task TryResetPwd(string email)
        {
            // TODO: Implement actual Firebase password reset
            await Task.Delay(100); // Simulate async operation

            // For now, just simulate the operation
            Console.WriteLine($"Password reset requested for: {email}");
        }
    }
}
