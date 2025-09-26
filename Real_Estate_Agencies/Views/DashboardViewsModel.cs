using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Real_Estate_Agencies.Model;

namespace Real_Estate_Agencies
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private string _currentDate;
        private string _currentTime;
        private DispatcherTimer _timer;

        // Quick Filters
        private string _selectedPropertyType;
        private string _selectedStatus;
        private string _selectedDateRange;

        public DashboardViewModel()
        {
            // Initialize date and time
            CurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
            CurrentTime = DateTime.Now.ToString("hh:mm:ss tt");

            // Start timer for ticking seconds
            _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            _timer.Tick += (s, e) =>
            {
                CurrentDate = DateTime.Now.ToString("MMMM dd, yyyy");
                CurrentTime = DateTime.Now.ToString("hh:mm:ss tt");
            };
            _timer.Start();



            // KPI Sample Data
            DashboardData = new DashboardKpi
            {
                SalesCount = 10,
                AgentCount = 4,
                Commission = 15000,
                PropertiesListed = 20,
                NewProperties = 8,
                NewPropertiesProgress = 50,
                AvgCommission = 12000,
                AvgCommissionProgress = 25,
                TopCategorySales = 15,
                TopCategoryProgress = 75
            };

            // Top Properties Sample
            OnSellProperties = new List<PropertyModel>
            {
                new PropertyModel { Id=1, Code="PROP-10001", Name="Condo A", PropertyType="Condo", Location="Makati", Price=120000, Category="Residential", Status="On Sale", ImagePath="C:\\Users\\maria\\Downloads\\iStock-1330454158-1-1.jpg" },
                new PropertyModel { Id=2, Code="PROP-10002", Name="House B", PropertyType="House", Location="Taguig", Price=200000, Category="Residential", Status="Sold", ImagePath="C:\\Users\\maria\\Downloads\\frames-for-your-heart-2d4lAQAlbDA-unsplash.jpg" },
                new PropertyModel { Id=3, Code="PROP-10003", Name="Lot C", PropertyType="Lot", Location="Quezon City", Price=95000, Category="Residential", Status="Rented", ImagePath="C:\\Users\\maria\\Downloads\\vacant-lot-presumably-mactan-island-cebu-philippines-residential-homes-visible-background-84593803.jpg" }
            };

            // Pie Chart
            PieSeries = new ISeries[]
            {
                new PieSeries<double> { Values = new double[] { 50 }, Name="Available" },
                new PieSeries<double> { Values = new double[] { 30 }, Name="Sold" },
                new PieSeries<double> { Values = new double[] { 20 }, Name="Rented" }
            };

            // Sales Over Time
            SalesOverTimeSeries = new ISeries[]
            {
                new LineSeries<double> { Values = new double[] { 5,7,3,8,10,12,15,14,18,20,22,25 }, Name="Sales" }
            };
            SalesXAxes = new Axis[] { new Axis { Labels = new[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" } } };
            SalesYAxes = new Axis[] { new Axis { Name = "Sales Count" } };

            // Agent Performance
            AgentPerformanceSeries = new ISeries[]
            {
                new ColumnSeries<double> { Values = new double[] { 50000 }, Name="Agent A" },
                new ColumnSeries<double> { Values = new double[] { 40000 }, Name="Agent B" },
                new ColumnSeries<double> { Values = new double[] { 30000 }, Name="Agent C" },
                new ColumnSeries<double> { Values = new double[] { 25000 }, Name="Agent D" }
            };
            AgentPerformanceXAxes = new Axis[] { new Axis { Labels = new[] { "Agent A", "Agent B", "Agent C", "Agent D" } } };
            AgentPerformanceYAxes = new Axis[] { new Axis { Name = "Sales ($)", MinLimit = 0 } };

            // Recent Sales Table
            RecentSales = new List<SaleRecord>
            {
                new SaleRecord { Property="Condo A", Agent="Agent A", Client="John Doe", Amount="$120,000", Date="2025-01-12" },
                new SaleRecord { Property="House B", Agent="Agent B", Client="Jane Smith", Amount="$200,000", Date="2025-02-08" },
                new SaleRecord { Property="Lot C", Agent="Agent C", Client="Mark Lee", Amount="$95,000", Date="2025-03-18" }
            };

            // Tasks / To-Do Panel
            Tasks = new List<TaskItem>
            {
                new TaskItem { Description="Follow-up with Client: John Doe", IsCompleted=false },
                new TaskItem { Description="Approve new listing: Condo A", IsCompleted=false },
                new TaskItem { Description="Update property status: House B", IsCompleted=false },
                new TaskItem { Description="Follow-up with Client: Jane Smith", IsCompleted=false }
            };
        }

        // KPI
        public DashboardKpi DashboardData { get; set; }

        // Top Properties
        public List<PropertyModel> OnSellProperties { get; set; }

        // Charts
        public ISeries[] PieSeries { get; set; }
        public ISeries[] SalesOverTimeSeries { get; set; }
        public Axis[] SalesXAxes { get; set; }
        public Axis[] SalesYAxes { get; set; }
        public ISeries[] AgentPerformanceSeries { get; set; }
        public Axis[] AgentPerformanceXAxes { get; set; }
        public Axis[] AgentPerformanceYAxes { get; set; }

        // Recent Sales
        public List<SaleRecord> RecentSales { get; set; }

        // Header
        public string CurrentDate { get => _currentDate; set { _currentDate = value; OnPropertyChanged(nameof(CurrentDate)); } }
        public string CurrentTime { get => _currentTime; set { _currentTime = value; OnPropertyChanged(nameof(CurrentTime)); } }

        // Tasks / To-Do Panel
        public List<TaskItem> Tasks { get; set; }

        // Quick Filters / Search
        public List<string> PropertyTypes { get; set; }
        public string SelectedPropertyType { get => _selectedPropertyType; set { _selectedPropertyType = value; OnPropertyChanged(nameof(SelectedPropertyType)); } }

        public List<string> StatusOptions { get; set; }
        public string SelectedStatus { get => _selectedStatus; set { _selectedStatus = value; OnPropertyChanged(nameof(SelectedStatus)); } }

        public List<string> DateRanges { get; set; }
        public string SelectedDateRange { get => _selectedDateRange; set { _selectedDateRange = value; OnPropertyChanged(nameof(SelectedDateRange)); } }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    // KPI class
    public class DashboardKpi
    {
        public int SalesCount { get; set; }
        public int AgentCount { get; set; }
        public double Commission { get; set; }
        public int PropertiesListed { get; set; }
        public int NewProperties { get; set; }
        public double NewPropertiesProgress { get; set; }
        public double AvgCommission { get; set; }
        public double AvgCommissionProgress { get; set; }
        public int TopCategorySales { get; set; }
        public double TopCategoryProgress { get; set; }
    }

    public class SaleRecord { public string Property { get; set; } public string Agent { get; set; } public string Client { get; set; } public string Amount { get; set; } public string Date { get; set; } }
    public class TaskItem { public string Description { get; set; } public bool IsCompleted { get; set; } }
}