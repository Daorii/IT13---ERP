public class AgentPerformanceModel
{
    public int AgentId { get; set; }
    public string Name { get; set; }
    public double TotalSales { get; set; }
    public double Commission { get; set; }

    public double TotalRevenue { get; set; }

    // Display initials in circle
    public string Initials
    {
        get
        {
            if (string.IsNullOrWhiteSpace(Name)) return "";
            var parts = Name.Split(' ');
            if (parts.Length == 1) return parts[0][0].ToString().ToUpper();
            return (parts[0][0].ToString() + parts[1][0].ToString()).ToUpper();
        }
    }
}
