using EShopping.Models;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations.Schema;

namespace EShopping.Areas.UI.Controllers
{
	public class Cart
	{
		public int Id { get; set; }
		public String? CartId { get; set; } //this is what is saved to sessions
		[ForeignKey("ProductId")]
		public int ProductId { get; set; }
		public int count { get; set; }
		public System.DateTime DateCreated { get; set; }
		public virtual Product? Product { get; set; }

	}
}
