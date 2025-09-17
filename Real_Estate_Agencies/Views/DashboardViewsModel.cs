using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies
{
    public class DashboardViewModel : BaseViewModel
    {
        public ObservableCollection<PropertyModel> OnSellProperties { get; set; } = new();
        public IEnumerable<ISeries> PieSeries { get; set; } = new List<ISeries>();
        public IEnumerable<ISeries> SalesOverTimeSeries { get; set; } = new List<ISeries>();
        public DashboardData DashboardData { get; set; } = new DashboardData();
        public string CurrentDate => System.DateTime.Now.ToShortDateString();
        public string CurrentTime => System.DateTime.Now.ToShortTimeString();

        public DashboardViewModel()
        {
            OnSellProperties.Add(new PropertyModel { Name = "Sunset Villa", Location = "Tagaytay", PropertyType = "House", Category = "Luxury", Status = "On Sale", Price = 2500000, ImagePath = "Images/villa1.jpg" });
            OnSellProperties.Add(new PropertyModel { Name = "City Apartment", Location = "Makati", PropertyType = "Condo", Category = "Residential", Status = "On Sale", Price = 1200000, ImagePath = "Images/apartment1.jpg" });

            PieSeries = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 40 }, Name = "House" },
                new PieSeries<double> { Values = new double[] { 35 }, Name = "Condo" },
                new PieSeries<double> { Values = new double[] { 25 }, Name = "Lot" }
            };

            SalesOverTimeSeries = new ISeries[]
            {
                new ColumnSeries<int> { Values = new int[] { 5, 10, 7, 12, 8, 15 } }
            };

            DashboardData = new DashboardData
            {
                SalesCount = 42,
                AgentCount = 8,
                Commission = 125000.50m,
                PropertiesListed = OnSellProperties.Count
            };
        }
    }

    public class DashboardData
    {
        public int SalesCount { get; set; }
        public int AgentCount { get; set; }
        public decimal Commission { get; set; }
        public int PropertiesListed { get; set; }
    }
}
