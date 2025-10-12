// AgentStatsModel.cs (put in Model folder)
namespace Real_Estate_Agencies.Model
{
    public class AgentStatsModel
    {
        public int TotalSales { get; set; }
        public double TotalPropertyValue { get; set; }
        public double ReleasedCommission { get; set; }
        public double CurrentCommission { get; set; }
        public int DealsThisMonth { get; set; }
    }
}
