using System;

namespace Real_Estate_Agencies
{
    public class AgentModel
    {
        public int EmployeeId { get; set; }    // Matches TxtEmployeeId
        public string FirstName { get; set; }  // Matches TxtFirstName
        public string LastName { get; set; }   // Matches TxtLastName
        public string ContactInfo { get; set; }// Matches TxtContactInfo
        public DateTime HireDate { get; set; } // Matches DpHireDate
        public decimal SalaryRate { get; set; }// Matches TxtSalaryRate
    }
}
