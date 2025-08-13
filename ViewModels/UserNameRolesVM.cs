using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Helpers;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.MVVM;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel.Communication;

namespace CFAN.SchoolMap.ViewModels
{
    public class UserNameRolesVM : BaseVM
    {
        private readonly Dictionary<Role, bool> _roles;

        public User User { get; set; }

        public UserNameRolesVM()
        {
            SaveCmd = new SafeCommand(Save);
            _roles = new Dictionary<Role, bool>
            {
                {Role.Schools_add, false},
                {Role.Schools_add_users, false},
                {Role.Admin, false},
                {Role.Outreaches_set_roles, false},
                {Role.Schools_set_roles, false},
                {Role.Tester, false},
                {Role.Schools_visit, false},
                {Role.Outreaches_add, false},
                {Role.Outreaches_add_users, false},
                {Role.Outreaches_visit, false},
                {Role.Schools_view, false},
                {Role.Outreaches_view, false}
            };
        }
        
        private string Email { get; set; }

        protected override async Task OnLoadData(Dictionary<string, string> pars)
        {
            var email = Param.GetOrDefault<string>(pars, nameof(Email));
            if (email == null)
            {
                throw new ApplicationException("No email given");
            }

            User = await Repository.GetUserBy(email);

            if (User?.Roles != null)
            {
                foreach (var role in User?.Roles)
                {
                    _roles[role] = true;
                }
            }

            Refresh();
        }

        protected override void Refresh()
        {
            base.Refresh();
            Notify(nameof(User));

            Notify(nameof(Add_schools));
            Notify(nameof(Add_markets));
            Notify(nameof(Add_users_markets));
            Notify(nameof(Add_users_schools));
            Notify(nameof(IsAdmin));
            Notify(nameof(Visit_schools));
            Notify(nameof(Visit_markets));
            Notify(nameof(View_schools));
            Notify(nameof(View_markets));
            Notify(nameof(IsSetRolesSchools));
            Notify(nameof(IsSetRolesMarkets));
            Notify(nameof(IsTester));

            Notify(nameof(IsSchoolRolesEnabled));
            Notify(nameof(IsMarketRolesEnabled));
            Notify(nameof(IsCurrentUserAdmin));
        }

        public Command SaveCmd { get; }

        private async Task Save()
        {
            var list = new List<Role>();
            foreach (var roleBool in _roles)
            {
                if (roleBool.Value) list.Add(roleBool.Key);
            }

            User.Roles = list.ToArray();

            await Repository.AddOrUpdateUser(User);
            ViewManager.CloseCurrentView();

            if (User.Email == Auth.User.Email)
            {
                Messaging.Send(Message.MyRolesChanged);
            }
            else
            {
                Messaging.Send(Message.RolesChanged);
            }
        }

        public bool Add_schools
        {
            get => _roles[Role.Schools_add];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Schools_set_roles))
                    _roles[Role.Schools_add] = value;
                Notify();
            }
        }

        public bool Add_markets
        {
            get => _roles[Role.Outreaches_add];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Outreaches_set_roles))
                    _roles[Role.Outreaches_add] = value;
                Notify();
            }
        }

        public bool Add_users_schools
        {
            get => _roles[Role.Schools_add_users];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Schools_set_roles))
                    _roles[Role.Schools_add_users] = value;
                Notify();
            }
        }

        public bool Add_users_markets
        {
            get => _roles[Role.Outreaches_add_users];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Outreaches_set_roles))
                _roles[Role.Outreaches_add_users] = value;
                Notify();
            }
        }

        public bool Visit_schools
        {
            get => _roles[Role.Schools_visit];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Schools_set_roles))
                    _roles[Role.Schools_visit] = value;
                Notify();
            }
        }

        public bool Visit_markets
        {
            get => _roles[Role.Outreaches_visit];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Outreaches_set_roles))
                    _roles[Role.Outreaches_visit] = value;
                Notify();
            }
        }

        public bool IsSetRolesSchools
        {
            get => _roles[Role.Schools_set_roles];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Schools_set_roles))
                    _roles[Role.Schools_set_roles] = value;
                Notify();
            }
        }

        public bool IsSetRolesMarkets
        {
            get => _roles[Role.Outreaches_set_roles];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Outreaches_set_roles))
                    _roles[Role.Outreaches_set_roles] = value;
                Notify();
            }
        }

        public bool IsTester
        {
            get => _roles[Role.Tester];
            set
            {
                if (HasRole(Role.Admin))
                    _roles[Role.Tester] = value;
                Notify();
            }
        }

        public bool IsAdmin
        {
            get => _roles[Role.Admin];
            set
            {
                if (HasRole(Role.Admin))
                    _roles[Role.Admin] = value;
                Notify();
            }
        }

        public bool View_schools
        {
            get => _roles[Role.Schools_view];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Schools_set_roles))
                    _roles[Role.Schools_view] = value;
                Notify();
            }
        }

        public bool View_markets
        {
            get => _roles[Role.Outreaches_view];
            set
            {
                if (HasRole(Role.Admin) || HasRole(Role.Outreaches_set_roles))
                    _roles[Role.Outreaches_view] = value;
                Notify();
            }
        }

        protected override void RefreshRoles()
        {
            Notify();
        }

        public bool IsSchoolRolesEnabled => HasRole(Role.Schools_set_roles);
        public bool IsMarketRolesEnabled => HasRole(Role.Outreaches_set_roles);
        public bool IsCurrentUserAdmin => HasRole(Role.Admin);

        public bool CanSave => IsSchoolRolesEnabled || IsMarketRolesEnabled || IsCurrentUserAdmin;
    }
}
