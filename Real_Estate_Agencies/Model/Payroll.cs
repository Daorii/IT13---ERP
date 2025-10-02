using System;

namespace Real_Estate_Agencies
{
    public class Payroll
    {
        public int PayrollId { get; set; }
        public int AgentId { get; set; }
        public string PayPeriod { get; set; }
        public decimal CommissionSalary { get; set; }
        public decimal NetPay { get; set; }

        // Display properties for UI
        public string PayrollIDDisplay => "PR" + PayrollId.ToString().PadLeft(3, '0');
        public string AgentIDDisplay => "A" + AgentId.ToString().PadLeft(3, '0');
    }
}
