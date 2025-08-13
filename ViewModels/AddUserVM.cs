using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.MVVM;
using CFAN.SchoolMap.Services;
using System;
using CFAN.SchoolMap.Services.Auth;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.Storage;
using Microsoft.Maui.ApplicationModel.Communication;

namespace CFAN.DecapolisFinance.ViewModels
{
    public class AddUserVM : BaseVM
    {
        private string _email;
        private string _password;
        private List<RoleBoolean> _roles;
        private List<Role> _availibleRoles;
        private Role[] _marketRoles;
        private Role[] _schoolRoles;
        public Command AddCmd { get; }
        public Command GeneratePasswordCmd { get; }

        public AddUserVM()
        {
            AddCmd = new SafeCommand(AddUser);
            GeneratePasswordCmd = new SafeCommand(GeneratePassword);
            SetUp();
        }

        private void SetUp()
        {
            if (HasRole(Role.Outreaches_add_users))
            {
                _marketRoles = new[] {
                    Role.Outreaches_add,
                    Role.Outreaches_view,
                    Role.Outreaches_visit
                }
                ;
            }
            else
            {
                _marketRoles = new Role[0];
            }

            if (HasRole(Role.Schools_add_users))
            {
                _schoolRoles = new[] {
                    Role.Schools_add,
                    Role.Schools_view,
                    Role.Schools_visit
                };
            }
            else
            {
                _schoolRoles = new Role[0];
            }

            _availibleRoles = (new[] { _schoolRoles, _marketRoles }).SelectMany(r => r).ToList();

            _roles = new();
            foreach (var role in _availibleRoles)
            {
                _roles.Add(new RoleBoolean(role) { Value = false});
            }

            Notify(nameof(Roles));
        }

        protected override void RefreshRoles()
        {
            base.RefreshRoles();
            SetUp();
        }

        public string Name { get; set; }

        public string Note { get; set; }

        public string Email
        {
            get => _email;
            set => _email = value.ToLowerInvariant();
        }

        public string Pwd
        {
            get => _password;
            set
            {
                _password = value;
                Notify(nameof(Pwd));
            }
        }

        public async Task GeneratePassword()
        {
            if (Name.IsNullOrWhiteSpace())
            {
                await Dialogs.ConfirmAsync("Fill the user's name!");
                return;
            }
            Pwd = $"{GetTwoRandomDigits()}*red!{ExtractInitials(Name)}";
        }

        private string GetTwoRandomDigits()
        {
            var rand = new Random();
            return rand.Next(10).ToString() + rand.Next(10).ToString();
        }

        private string ExtractInitials(string name)
        {
            var names = name.Split(' ');
            var initials = names.Select(x => x.ToLower().FirstOrGivenValue('g'));
            return string.Join("", initials);
        }

        public async Task AddUser()
        {
            bool reset = false;
            if (Auth.IsTester) return;

            if (Pwd.IsNullOrWhiteSpace())
            {
                await Dialogs.ConfirmAsync("Fill the password!");
                return;
            }

            if (Email.IsNullOrWhiteSpace())
            {
                await Dialogs.ConfirmAsync("Fill the user's email!");
                return;
            }

            if (Pwd.Length < 6)
            {
                await Dialogs.ConfirmAsync("The password must have at least 6 characters!");
                return;
            }

            if (await Repository.UserExists(Email))
            {
                var canProceed = await Dialogs.ConfirmAsync("User with this email already exists in the app! Would you like to reset his password? (permissions will not be changed)");
                if (!canProceed) return;
                reset = true;
            }
            else if (Name.IsNullOrWhiteSpace())
            {
                await Dialogs.ConfirmAsync("Fill the user's name!");
                return;
            }

            var roleList = new List<Role>();
            foreach (var roleBool in _roles)
            {
                if (roleBool.Value) roleList.Add(roleBool.Key);
            }

            if (roleList.Count == 0)
            {
                await Dialogs.ConfirmAsync("User must have at least one role!");
                return;
            }


            if (reset)
            {
                await Repository.ResetUserPassword(Email, Pwd);
            }
            else
            {
                var id = await Auth.TryCreateUser(Email, Pwd);
                if (id == null)
                {
                    await Dialogs.ConfirmAsync("Unable to create the user! Check your internet connection and the email address.");
                    return;
                }

                if (id == AuthResponse.EXISTS)
                {
                    await Dialogs.ConfirmAsync("User already created, but user setting is missing. Contact the administrator.");
                    return;
                }

                try
                {
                    var oldUser = await SecureStorage.GetAsync("UserEmail");
                    var oldPwd = await SecureStorage.GetAsync("UserPassword");
                    if (oldUser != null && oldPwd != null)
                    {
                        await Auth.LoginWithEmailPassword(oldUser, oldPwd);
                    }
                }
                catch (Exception ex)
                {
                    Dialogs.Toast("Unable to relogin!");
                }

                await Repository.AddOrUpdateUser(
                    new User {
                        Name = Name,
                        Id = id,
                        Email = Email,
                        Roles = roleList.ToArray(),
                        Note = Note
                    });               
            }

            var senderName = (await Repository.GetUserBy(Auth.User.Email))?.Name ?? "Administrator";

            var message =
                reset ?
$@"Hi,
Your CFAN Red Application account password was changed.
Your login name is {Email} and new password is {Pwd} . 
God bless you
{senderName}"
:
$@"Hi,
Your CFAN Red Application account was activated.
Login name is {Email} and password is {Pwd} . 
For more information how to install and use the application visit https://cfan-red-app3.webnode.co.uk/home/
God bless you
{senderName}";

            await EmailSender.SendEmail(
                "Your account in CFAN Red App",
                message,
                new List<string> { Email });

            ViewManager.CloseCurrentView();
        }

        public List<Role> AvailableRoles => _availibleRoles;

        public List<RoleBoolean> Roles => _roles;
    }

    public class RoleBoolean
    {
        public bool Value { get; set; } = false;

        public Role Key { get; set; }

        public RoleBoolean(Role key)
        {
            Key = key;
        }
    }
}
