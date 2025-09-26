using System;

namespace Real_Estate_Agencies.Model
{
    public class Incentive
    {
        public int IncentiveId { get; set; }   // Auto-increment, hidden in UI
        public int AgentId { get; set; }
        public string IncentiveType { get; set; }  // e.g., "Bonus", "Allowance"
        public decimal Amount { get; set; }
        public string Status { get; set; }     // Pending / Released / Cancelled
        public DateTime ReleaseDate { get; set; }
    }
}
