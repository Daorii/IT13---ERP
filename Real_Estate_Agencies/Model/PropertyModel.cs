using System;

namespace Real_Estate_Agencies.Model
{
    public class PropertyModel
    {

        public int PropertyId { get; set; }
        public int Id { get; set; }

        // Unique identifier you want to display (like PROP-1001)
        public string Code { get; set; }

        public string Name { get; set; }
        public string Location { get; set; }
        public string PropertyType { get; set; }
        public string Category { get; set; }

        // Property price
        public double Price { get; set; }

        public string Status { get; set; }

        // Display order number (like row # in the table)
        public int No { get; set; }

        // Path to the property image stored locally
        public string ImagePath { get; set; }

        // Optional: store image directly as byte[] if you want DB storage
        public byte[] Image { get; set; }

        // ✅ New fields
        public DateTime? SaleDate { get; set; }     // When the property was sold
        public string PaymentMode { get; set; }     // Cash, Bank Financing, etc.
    }
}
