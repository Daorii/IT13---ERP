
namespace Real_Estate_Agencies.Model
{
    public class Client
    {
        public int ClientId { get; set; }                  // DB identity column
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ContactInfo { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string PreferredPropertyType { get; set; } = string.Empty;
    }
}
