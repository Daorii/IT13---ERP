using System;

namespace Real_Estate_Agencies.Model
{
    public class CommissionReleaseModel
    {
        public int ReleaseID { get; set; }
        public int AgentID { get; set; }
        public string AgentName { get; set; } // for display
        public DateTime ReleaseDate { get; set; }

        public decimal ReleasedAmount { get; set; }
    }
}
