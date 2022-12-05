using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Framework;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace EShopping.Models
{
	public class Product
	{
		public int Id { get; set; }
		
		[Display(Name ="Name")]
		[Required]
		public String Name { get; set; }
		[Required]
		public Decimal Price { get; set; }
       
        public int Quanity { get; set; }
        public String? Image { get; set; }
		[Display(Name="Color")]
		public String? ProductColor { get; set; }
		[Required]
		[Display(Name ="Available")]
		public bool IsAvailable { get; set; }
		[Required]
		[Display(Name ="Type")] //we hide we dont wan't to show the id
		public int ProductTypeId { get; set; }
		[ForeignKey("ProductTypeId")]
		public virtual ProductType? ProductType { get; set; }
		//public IEnumerable<SelectListItem> AllProductTypes { set; get; }

		[Required]
		[Display(Name = "Tag")]
		public int ProductTagId { get; set; }
		[ForeignKey("ProductTagId")]
		public virtual ProductTag? ProductTag { get; set; }
		//lazy loading enables lazy loading of the class Product tag
		//loads the referenced entity when it is first accessed
	}
}
