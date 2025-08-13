using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Controls;
using Microsoft.Maui;

namespace CFAN.SchoolMap.Views
{
    public partial class MarketStatisticsPage : ContentPage
    {
        public MarketStatisticsPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //var entries = new[]
            //{
            //    new ChartEntry(212)
            //    {
            //        Label = "19",
            //        ValueLabel = "212",
            //        Color = SKColor.Parse("#2c3e50")
            //    },
            //    new ChartEntry(400)
            //    {
            //        Label = "20",
            //        ValueLabel = "400",
            //        Color = SKColor.Parse("#77d065")
            //    },
            //    new ChartEntry(550)
            //    {
            //        Label = "21",
            //        ValueLabel = "550",
            //        Color = SKColor.Parse("#b455b6")
            //    },
            //    new ChartEntry(700)
            //    {
            //        Label = "22",
            //        ValueLabel = "700",
            //        Color = SKColor.Parse("#3498db")
            //    }
            //};
            //var Chart = new LineChart {Entries = entries};
            //Chart.LineSize = 10;
            //Chart.LabelTextSize = 60;
            //Chart.LabelOrientation = Orientation.Horizontal;
            //Chart.ValueLabelOrientation = Orientation.Vertical;
            //Chart.Margin = 10;
            //Chart.EnableYFadeOutGradient = true;
            //Chart.LineMode = LineMode.Straight;
            //chartView.Chart = Chart;
        }
    }
}