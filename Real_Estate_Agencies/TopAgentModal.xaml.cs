using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Real_Estate_Agencies
{
    public partial class TopAgentModal : UserControl
    {
        public Action CloseClicked;

        public TopAgentModal()
        {
            InitializeComponent();
        }

        public void SetAgentData(string name, int sales, string revenue, int deals, List<Deal> recentDeals)
        {
            AgentNameText.Text = name;
            AgentSalesText.Text = $"Sales: {sales}";
            AgentRevenueText.Text = $"Revenue: {revenue}";
            AgentDealsText.Text = $"Deals this month: {deals}";
            AgentCommissionText.Text = $"Commission: {sales * 0.05:C}";

            RecentDealsGrid.ItemsSource = recentDeals;

            // Example chart data
            BarChart.Series = new ISeries[]
            {
                new ColumnSeries<int> { Values = new int[] { 5, 8, 10, 7, 12, 6, 9, 15, 11, 8, 10, 14 } }
            };

            LineChart.Series = new ISeries[]
            {
                new LineSeries<int> { Values = new int[] { 50, 55, 60, 58, 70, 65, 80, 75, 85, 90, 95, 100 } }
            };
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            CloseClicked?.Invoke();
        }
    }

    public class Deal
    {
        public string Property { get; set; }
        public string Price { get; set; }
        public string Date { get; set; }
    }
}
