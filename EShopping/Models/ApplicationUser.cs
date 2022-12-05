using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShopping.Models
{
    public class ApplicationUser :IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string? PhoneNumber { get; set; }
        public string? StreetAddress { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? PostalCode { get; set;}
        public int? CompanyId { get; set;}
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public Company company { get; set;}
        public string Role { get; set; }

    }
}
