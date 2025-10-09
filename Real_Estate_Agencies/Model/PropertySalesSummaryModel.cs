namespace Real_Estate_Agencies.Model
{
    public class PropertySalesSummaryModel
    {
        public int PropertyID { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string PropertyType { get; set; }
        public string Category { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public string ImagePath { get; set; }
        public int TotalSales { get; set; }
    }
}
