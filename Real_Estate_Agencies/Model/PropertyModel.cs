namespace Real_Estate_Agencies.Model
{
    public class PropertyModel
    {
        public int Id { get; set; }              // Unique identifier
        public string Code { get; set; }         // Property code, e.g., "P001"
        public string Name { get; set; }         // Property name
        public string Location { get; set; }     // Property location
        public string PropertyType { get; set; } // House, Condo, Office, etc.
        public string Category { get; set; }     // Residential, Commercial, etc.
        public double Price { get; set; }        // Price
        public string Status { get; set; }       // On Sale, Pending, Sold
        public string ImagePath { get; set; }    // Path to property image
        public int No { get; set; }              // Optional: property number
    }
}
