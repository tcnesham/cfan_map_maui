using CFAN.SchoolMap.Maui.Services.Auth;
using CFAN.SchoolMap.Services.Auth;

namespace CFAN.SchoolMap.Maui.Services.Auth
{
    public class CurrentUserChangedMessage(IAuth sender)
    {
        public IAuth Sender { get; } = sender;
    }
}
