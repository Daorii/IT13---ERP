using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Real_Estate_Agencies.Data;
using SkiaSharp;
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

        public void SetAgentData(string agentName, int totalSales, string releasedCommissionFormatted, string currentCommissionFormatted, int dealsThisMonth, List<Deal> recentDeals, int[] monthlySales = null)
        {
            AgentNameText.Text = agentName;
            AgentSalesText.Text = $"Sales: {totalSales}";
            AgentRevenueText.Text = $"Released: {releasedCommissionFormatted}"; // revenue == released commission
            AgentDealsText.Text = $"Deals this month: {dealsThisMonth}";
            AgentCommissionText.Text = $"Current Commission: {currentCommissionFormatted}";

            // Set recent deals
            RecentDealsGrid.ItemsSource = recentDeals;

            // Chart: if monthlySales provided, use them; otherwise show totalSales as fallback
            if (monthlySales != null && monthlySales.Length == 12)
            {
                BarChart.Series = new ISeries[]
                {
            new ColumnSeries<int>
            {
                Values = monthlySales,
                Name = "Monthly Sales",
                Fill = new SolidColorPaint(SKColors.DeepSkyBlue)
            }
                };
            }
            else
            {
                BarChart.Series = new ISeries[]
                {
            new ColumnSeries<int>
            {
                Values = new int[] { totalSales },
                Name = "Total Sales",
                Fill = new SolidColorPaint(SKColors.DeepSkyBlue)
            }
                };
            }
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
