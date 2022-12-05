using System.ComponentModel.DataAnnotations;

namespace EShopping.Models
{
    public class Company
    {
        public int CompanyId { get; set; }
        [Required]
        public string Name { get; set;}
        public string? StreetAddress { get; set; }
        public string? City { get; set;}
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
