using CFAN.Common.WPF;
using CFAN.SchoolMap.Model;
using CFAN.SchoolMap.MVVM;
using CFAN.SchoolMap.Views;
#if IOS || MACCATALYST
using Foundation;
#endif
using ISO3166;
using Plugin.CloudFirestore;
using CFAN.SchoolMap.Maui.Database;
using CFAN.SchoolMap.Maui.Model;

namespace CFAN.SchoolMap.ViewModels
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public class AdminViewModel : BaseVM
    {
        public Command StartMonthlyRutineCommand { get; }

        public Command AddNewUserCommand { get; }

        public Command UsersCommand { get; }

        public Command OtherCmd { get; }

        public AdminViewModel()
        {
            StartMonthlyRutineCommand = new SafeCommand(StartMonthlyRutine);
            AddNewUserCommand = new SafeCommand(() => ViewManager.Navigate<AddUserPage>());
            UsersCommand = new SafeCommand(() => 
                ViewManager.Navigate<RolesPage>());

            OtherCmd = new SafeCommand(Other);
        }

        public bool CanAddUsers => HasRole(Role.Schools_add_users) || HasRole(Role.Outreaches_add_users) || HasRole(Role.Admin);

        public bool CanSetRoles => HasRole(Role.Schools_set_roles) || HasRole(Role.Outreaches_set_roles) || HasRole(Role.Admin);

        public bool CanCompressData => HasRole(Role.Admin);

        protected override void RefreshRoles()
        {
            base.RefreshRoles();
            Notify(nameof(CanAddUsers));
            Notify(nameof(CanSetRoles));
            Notify(nameof(CanCompressData));
        }

        private async Task StartMonthlyRutine(object obj)
        {
            var repository = DependencyService.Get<IRepository>();
            foreach (var c in Country.List.Select(c=>c.ThreeLetterCode).OrderBy(s=>s))
            {
                await Compress<PlacePoint>(await repository.SchoolsCountryHasData(c), c, s => repository.SchoolsLoadCountry(s, true));
                await Compress<MarketPoint>(await repository.MarketsCountryHasData(c), c, s => repository.MarketsLoadCountry(s, true));
            }
            await DownsizeSchools();
        }

        private async Task Compress<Tpoint>(bool hasData, string country, Func<string, Task> loadCountryTask)
        {
            var newPlacesDoc = await CrossCloudFirestore.Current.Instance
                                .Collection(typeof(Tpoint).Name)
                                .WhereEqualsTo(nameof(BasePoint.Country), country)
                                .LimitTo(1)
                                .GetAsync();

            if (hasData || newPlacesDoc.Count > 0)
            {
                using (Dialogs.Loading("Compressing " + country))
                {
                    await loadCountryTask(country);
                }
            }
        }

        private async Task DownsizeSchools()
        {
            var stat = await Repository.GetStatistics();
            foreach (var s in stat)
            {
                if (s.Schools > 20000)
                {
                    using (Dialogs.Loading("downsizing " + s.CountryCode))
                    {
                        await Repository.DownsizeSchools(s.CountryCode);
                    }
                }
            }
        }

        public async Task Other()
        {
            //var users = await Repository.GetUsers();
            //foreach (var user in users)
            //{
            //    if (user.Roles == null || !user.Roles.Any())
            //    {
            //        user.Roles = [Role.Schools_view, Role.Schools_add, Role.Schools_visit];
            //        await Repository.AddOrUpdateUser(user);
            //    }
            //}
        }

        //        public async Task Import()
        //        {
        //            var text = @"Aramis Albino ;albinoaramis@gmail.com
        //David Uher;info@hissonministries.com";
        //            foreach (var line in text.Split('\n'))
        //            {
        //                var split = line.Split(';');
        //                var name = split[0].Trim();
        //                var email = split[1].Trim().ToLower();
        //                if (string.IsNullOrWhiteSpace(name)) name = email;

        //                await Repository.AddOrUpdateUser(new User { Email = email, Name = name });
        //            }

        //        }
    }
}
