using System.ComponentModel.DataAnnotations;

namespace EShopping.Models
{
    public class ProductType
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="The product type name is mandatory")]
        [Display(Name ="Type Name")]
        public String TypeName { get; set; }
    }
}
