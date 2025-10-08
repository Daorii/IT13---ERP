namespace Real_Estate_Agencies.Models
{
    public class CommissionDisplayItem
    {
        public string CommissionID { get; set; }
        public string SalesID { get; set; }
        public string AgentID { get; set; }
        public string AgentName { get; set; } // ✅ this property fixes the error
        public string CommissionAmount { get; set; }
        public string ReleaseDate { get; set; }
    }
}
