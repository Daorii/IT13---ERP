using System;

namespace Real_Estate_Agencies.Models
{
    public class Commission
    {
        public int CommissionId { get; set; }   // Auto-increment
        public int SalesId { get; set; }        // FK → Sale
        public int AgentId { get; set; }        // FK → Agent
        public int PaymentId { get; set; }      // Auto-increment
        public decimal CommissionRate { get; set; }
        public decimal CommissionAmount { get; set; }
        public string Status { get; set; }
        public DateTime ReleaseDate { get; set; }
    }
}
