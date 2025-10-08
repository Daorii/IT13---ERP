using System;

namespace Real_Estate_Agencies.Model
{
    public class Sale
    {
        public int SaleId { get; set; }
        public int ClientId { get; set; }
        public int PropertyId { get; set; }
        public string PropertyName { get; set; } = string.Empty;

        public int AgentId { get; set; }
        public DateTime SaleDate { get; set; }
        public string PaymentMode { get; set; } = string.Empty; // "One-time Payment" or "Installment"

        // ----- Added for display in SalesPage -----
        public string ClientName { get; set; } = string.Empty;
        public string PropertyType { get; set; } = string.Empty;
        public string AgentName { get; set; } = string.Empty;
    }
}
