using System;

namespace Real_Estate_Agencies
{
    public class Payroll
    {
        public int PayrollId { get; set; }
        public int AgentId { get; set; }
        public string Period { get; set; }
        public decimal BasicPay { get; set; }
        public decimal Deductions { get; set; }
        public decimal NetPay { get; set; }
        public DateTime ReleaseDate { get; set; }

        // Display properties for the UI (read-only)
        public string PayrollID => "PR" + PayrollId.ToString().PadLeft(3, '0');
        public string AgentID => "A" + AgentId.ToString().PadLeft(3, '0');
    }
}