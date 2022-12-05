


using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace EShopping.Models
{


    public class ProductTag
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage ="Tag Name Is Required")]
        [Display(Name ="Product Tag Name")]
        public string? Name { get; set; }
    }
}
