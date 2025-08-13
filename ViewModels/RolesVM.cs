using System.Collections.Generic;
using CFAN.Common.WPF;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.MVVM;
using CFAN.SchoolMap.Views;
using System.Linq;
using CFAN.SchoolMap.Helpers;
using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;
using Microsoft.Maui.ApplicationModel.Communication;

namespace CFAN.SchoolMap.ViewModels
{
    public class RolesVM : BaseVM
    {
        private User _selectedItem;
        private string _searchString;
        private List<User> _items;


        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                Notify(nameof(Items));
            }
        }

        protected override void PermissionChanged()
        {
            Refresh();
        }

        protected override async void Refresh()
        {
            base.Refresh();
            _items =  (await Repository.GetUsers()).OrderBy(x => x.Name).ToList();
            //if (Auth.IsAdmin)
            //{
            //    foreach (var user in _items)
            //    {
            //        var list = new List<Role>
            //        {
            //            Role.Schools_add,
            //            Role.Schools_view,
            //            Role.Schools_visit
            //        };
            //        user.Roles = list.ToArray();
            //        await Repository.AddOrUpdateUser(user);
            //    }
            //}
            Notify(nameof(Items));
            _selectedItem = null;
            Notify(nameof(SelectedItem));
        }

        public IEnumerable<User> Items => _items?.Where(x =>
            string.IsNullOrEmpty(_searchString)
            || x.NameAndEmail.ToLower().Contains(SearchString.ToLower()))
            .OrderBy(x => x.Name);

        private string Email { get; set; }

        public User SelectedItem
        {
            get => _selectedItem;
            set
            {
                _selectedItem = value;
                if (value != null) 
                {
                    ViewManager.Navigate<UserNameRolesPage>(new Param(nameof(Email), value.Email));

                }
            }
        }

        public Command ItemClickCmd => new SafeCommand(ItemClick);

        private void ItemClick(object itemObj)
        {
            if (itemObj is User item)
            {
                SelectedItem = item;
            }
        }
    }
}
