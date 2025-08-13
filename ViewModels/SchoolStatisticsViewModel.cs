using System.Windows.Input;
using CFAN.Common.WPF;
using CFAN.SchoolMap.MVVM;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.BigQuery.V2;
using ISO3166;
#if IOS || MACCATALYST
using Foundation;
#endif
using System.Diagnostics;

namespace CFAN.SchoolMap.ViewModels
{
#if IOS || MACCATALYST
    [Preserve(AllMembers = true)]
#endif
    public class SchoolStatisticsViewModel : BaseVM
    {
        static GoogleCredential? _credential;

        private IStatDateDim? _selectedDim;
        private BigQueryClient? _bqClient;
        private bool _filterByCountry;
        private bool _filterByEmail;

        public ICommand RunCommand { get; }

        public bool IsAggregated { get; set; }

        public bool FilterByCountry
        {
            get => _filterByCountry;
            set => SetProperty(ref _filterByCountry, value);
        }

        public Country? SelectedCountry { get; set; }

        public bool FilterByEmail
        {
            get => _filterByEmail;
            set => SetProperty(ref _filterByEmail, value);
        }

        public string? SelectedEmail { get; set; }

        public StatValue[] Statistics { get; set; } = Array.Empty<StatValue>();

        public IStatDateDim? SelectedDim
        {
            get => _selectedDim;
            set => SetProperty(ref _selectedDim, value);
        }

        public IStatDateDim[] Dims { get; }

        public SchoolStatisticsViewModel()
        {
            Title = "About CfaN schools app";
            RunCommand = new SafeCommand(Run);
            Dims = new IStatDateDim[]
            {
                new StatDimDay(),
                new StatDimWeek(),
                new StatDimMonth(),
                new StatDimYear(),
            };
            SelectedDim = Dims.First();
            _ = LoginToBigQuery();
        }

        private async Task LoginToBigQuery()
        {
            try
            {
                using (Dialogs.Loading("Loading countries and users"))
                {
                    if (_credential == null)
                    {
                        using var stream = GetType().Assembly
                            .GetManifestResourceStream("CFAN.SchoolMap.BigQueryServiceKey.json");
                        if (stream == null) throw new Exception("Missing BigQueryServiceKey.json.");
                        _credential = GoogleCredential.FromStream(stream);
                    }

                    _bqClient = await BigQueryClient.CreateAsync("cfan-schools", _credential);

                    Emails = (await _bqClient.ExecuteQueryAsync(
                            "SELECT DISTINCT User FROM cfan-schools.Schools.Visits order by User", parameters: null))
                        .Select(r => r["User"].ToString())
                        .Where(email => !string.IsNullOrEmpty(email))
                        .ToArray()!;
                    SelectedEmail = Emails.FirstOrDefault();
                    Notify(nameof(Emails));

                    var countryCodes =
                        (await _bqClient.ExecuteQueryAsync("SELECT DISTINCT Country FROM cfan-schools.Schools.Visits", parameters: null))
                        .Select(r => r["Country"].ToString())
                        .ToArray();
                    Countries = Country.List.Where(c => countryCodes.Contains(c.ThreeLetterCode)).ToArray();
                    SelectedCountry = Countries.FirstOrDefault();
                    Notify(nameof(Countries));
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, false, null, "Statistics initiation failed!\n" + e.Message);
            }
        }

        public IEnumerable<Country> Countries { get; set; } = Enumerable.Empty<Country>();

        public string[] Emails { get; set; } = Array.Empty<string>();

        private async Task Run()
        {
            using (Dialogs.Loading("Computing statistics"))
            {
                try
                {
                    if (_bqClient == null)
                    {
                        //Debug.Fail("BigQuery client is not initialized.");

                        InvalidOperationException e = new InvalidOperationException("BigQuery client is not initialized.");
                        ExceptionHandler.HandleException(e, false);
                        return;
                    }

                    if (SelectedDim == null)
                    {
                        //Debug.Fail("No dimension is selected.");
                        InvalidOperationException e = new InvalidOperationException("No dimension is selected.");
                        ExceptionHandler.HandleException(e, false);
                        return;
                    }

                    var dateCol = SelectedDim.Column;

                    var sql =
                        @$"select
                            d.{dateCol} as date,
                            sum(d.Attendance) as Attendance,
                            sum(d.Decisions) as Decisions,
                            sum(d.Visit) as Visit
                            from(
                                SELECT DISTINCT *
                                FROM cfan-schools.Schools.Visits v
                            {CreateFilter()}
                                ) d
                            group by d.{dateCol}
                            having sum(d.Visit)>0
                            order by d.{dateCol}";

                    var results = await _bqClient.ExecuteQueryAsync(sql, parameters: null);

                    var data = results.Select(r => new StatValue(r, SelectedDim)).ToArray();
                    AggregateData(data);
                    Statistics = data.Reverse().ToArray();
                    Notify(nameof(Statistics));
                }
                catch (Exception e)
                {
                    ExceptionHandler.HandleException(e, false, null,
                        "Computing statistics failed. Check you internet connection or try it later.");
                }
            }

        }

        private void AggregateData(StatValue[] data)
        {
            if (IsAggregated && data.Length>1)
            {
                for (int i = 1; i < data.Length; i++)
                {
                    var p = data[i - 1];
                    var n = data[i];
                    n.Attendance += p.Attendance;
                    n.Visit += p.Visit;
                    n.Decisions += p.Decisions;
                }
            }
        }

        private string CreateFilter()
        {
            var filter = "";
            if (FilterByCountry && SelectedCountry!=null)
            {
                filter += $"v.Country = '{SelectedCountry.ThreeLetterCode}'";
            }

            if (FilterByEmail && SelectedEmail!=null)
            {
                if (filter != "") filter += " and ";
                filter += $"v.User = '{SelectedEmail}'";
            }

            if (filter != "") filter = "where " + filter;
            return filter;
        }
    }
}