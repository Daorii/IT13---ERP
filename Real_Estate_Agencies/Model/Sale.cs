using System;

namespace Real_Estate_Agencies.Model
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int ClientId { get; set; }
        public int PropertyId { get; set; }
        public int AgentId { get; set; }
        public DateTime SaleDate { get; set; }
        public string PaymentMode { get; set; } = string.Empty; // "One-time Payment" or "Installment"
    }
}
