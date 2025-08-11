using System.Collections.Generic;
using System.Linq;
using CFAN.SchoolMap.Enumerations;
using CFAN.SchoolMap.Helpers;
using Google.Apis.Bigquery.v2.Data;
using Newtonsoft.Json;
using Plugin.CloudFirestore.Attributes;
using CFAN.SchoolMap.Database;
using System;

namespace CFAN.SchoolMap.Model
{
    public class User
    {
        private string _name;
        private string _lastEditedByEmail;

        public string Id { get; set; }
        public string Email { get; set; }
        public string Note { get; set; }

        public string Name
        {
            get => _name;
            set
            {
                if (_name != value)
                {
                    _name = value;
                    SetChanged();
                }
            }
        }

        public string LastEditedByEmail
        {
            get => _lastEditedByEmail;
            set
            {
                _lastEditedByEmail = value;
                SetChanged();
            }
        }

        [Ignored]
        [JsonIgnore]
        public bool IsChanged { get; private set; }

        protected void SetChanged()
        {
            IsChanged = true;
        }

        public void ResetChanged()
        {
            IsChanged = false;
        }

        [Ignored]
        [JsonIgnore]
        public string NameAndEmail => Name + " (" + Email + ")";

        [Ignored]
        [JsonIgnore]
        public string RolesAsString
        {
            get
            {
                var roles = Roles.Safe().Select(r => r.ToString().Replace("_", " "));
                return string.Join(", ", roles);
            }
        }

        public override string ToString()
        {
            return Name;
        }


        [Ignored]
        [JsonIgnore]
        public string DebugInfo => Name;

        [Ignored]
        [JsonIgnore]
        public Role[] Roles { get; set; }

        public string RolesAsText
        {
            get => (Roles==null)
                ?String.Empty
                :string.Join(",", Roles.Select(role => ((int)role).ToString()));
            set => Roles = (value.IsNullOrWhiteSpace())
                ?new Role[0]
                :value.Split(',').Select(s => (Role)Enum.Parse(typeof(Role), s)).ToArray();
        }

        public bool HasModulRoles(Modul modul)
        {
            if (Roles == null) return false;
            switch (modul)
            {
                case Modul.None:
                    return true;
                case Modul.Schools:
                    return Roles.Contains(Model.Role.Schools_view)
                        || Roles.Contains(Model.Role.Schools_visit)
                        || Roles.Contains(Model.Role.Schools_add)
                        || Roles.Contains(Model.Role.Admin)
                        || Roles.Contains(Model.Role.Tester);
                case Modul.Markets:
                    return Roles.Contains(Model.Role.Outreaches_view)
                        || Roles.Contains(Model.Role.Outreaches_visit)
                        || Roles.Contains(Model.Role.Outreaches_add)
                        || Roles.Contains(Model.Role.Admin)
                        || Roles.Contains(Model.Role.Tester);
                default:
                    return false;
            }
        }

        public bool HasAdministrationRoles()
        {
            if (Roles == null) return false;
            return Roles.Contains(Model.Role.Admin)
                || Roles.Contains(Model.Role.Outreaches_set_roles)
                || Roles.Contains(Model.Role.Outreaches_add_users)
                || Roles.Contains(Model.Role.Schools_add_users)
                || Roles.Contains(Model.Role.Schools_set_roles);
        }
    }
}
