using System.Threading.Tasks;

using CFAN.SchoolMap.Model;

namespace CFAN.SchoolMap.Services.Auth
{
    public interface IAuth
    {
        Task<string> LoginWithEmailPassword(string email, string password);
        Task<string> TryCreateUser(string email, string pwd);
        bool LogOut();
        Task TryResetPwd(string email);

        bool IsSignIn { get; }
        bool IsTester { get; }
        bool IsAdmin { get; }

        void SetCurrentUserRoles(Role[] roles);
        FBUser User { get; }
    }

    public static class AuthResponse
    {
        public static string EXISTS = "EXISTS";
    }

    public class FBUser
    {
        public FBUser(string email, string id)
        {
            Email = email;
            Id = id;
        }

        public string Email { get; set; }
        public string Id { get; set; }

        public string Folder => Email.Replace("@", ".");
    }
}
