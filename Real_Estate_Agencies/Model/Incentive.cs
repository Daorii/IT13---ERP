using System;

namespace Real_Estate_Agencies.Model
{
    public class Incentive
    {
        public int IncentiveId { get; set; }   // Primary Key
        public int AgentId { get; set; }       // Foreign Key to Agents
        public string AgentName { get; set; } = string.Empty;  // ✅ for display
        public string IncentiveType { get; set; }
        public decimal Amount { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
