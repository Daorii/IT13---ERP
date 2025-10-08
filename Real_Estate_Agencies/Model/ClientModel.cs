using System;

namespace Real_Estate_Agencies.Model
{
    public class Client
    {
        // Basic client info
        public int ClientId { get; set; }                  // DB identity column
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
       

        // Profile / payment info
        public decimal Balance { get; set; } = 0m;          // Current balance
        public DateTime PaymentDate { get; set; } = DateTime.MinValue; // Last payment date
        public decimal AmountPaid { get; set; } = 0m;      // Amount paid
        public string PaymentType { get; set; } = string.Empty; // e.g., Cash, Credit
        public string Status { get; set; } = string.Empty; // Fully Paid / Partially Paid
    }
}
