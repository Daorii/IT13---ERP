namespace Real_Estate_Agencies
{
    public class ClientModel
    {
        public int ClientId { get; set; }                  // Matches TxtClientId
        public string FirstName { get; set; }             // Matches TxtFirstName
        public string LastName { get; set; }              // Matches TxtLastName
        public string ContactInfo { get; set; }           // Matches TxtContactInfo
        public string Address { get; set; }               // Matches TxtAddress
        public string PreferredPropertyType { get; set; } // Matches CmbPropertyType
    }
}
