using System;
using System.Collections.Generic;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace Real_Estate_Agencies.Models
{
    public class DashboardViewModel
    {
        public string CurrentDate { get; set; }
        public string CurrentTime { get; set; }

        public DashboardModel DashboardData { get; set; }
        public IEnumerable<ISeries> PieSeries { get; set; }
        public IEnumerable<PropertyModel> OnSellProperties { get; set; }

        public DashboardViewModel()
        {
            // Current date/time
            CurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
            CurrentTime = DateTime.Now.ToString("hh:mm tt");

            // Demo KPI values
            DashboardData = new DashboardModel
            {
                SalesCount = "15",
                AgentCount = "8",
                Commission = "$1200",
                PropertiesListed = "32"
            };

            // Pie chart sample
            PieSeries = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 10 }, Name="House A" },
                new PieSeries<double> { Values = new double[] { 20 }, Name="Condo B" },
                new PieSeries<double> { Values = new double[] { 30 }, Name="Villa C" },
            };

            // Property list sample
            OnSellProperties = new List<PropertyModel>
            {
                new PropertyModel { Name="House A", Price="$200,000", Status="For Sale", ImagePath="Images/house1.jpg" },
                new PropertyModel { Name="Condo B", Price="$120,000", Status="For Sale", ImagePath="Images/house2.jpg" },
                new PropertyModel { Name="Villa C", Price="$350,000", Status="For Sale", ImagePath="Images/house3.jpg" },
                new PropertyModel { Name="Apartment D", Price="$90,000", Status="For Sale", ImagePath="Images/house4.jpg" },
                new PropertyModel { Name="Townhouse E", Price="$180,000", Status="For Sale", ImagePath="Images/house5.jpg" }
            };
        }
    }
}
