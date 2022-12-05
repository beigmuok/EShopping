using EShopping.Data;
using EShopping.HelperClasses;
using EShopping.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Nodes;

namespace EShopping.Areas.UI.Controllers
{
	[Area("UI")]
	public class CartManagerController : Controller
	{
		public readonly ApplicationDbContext _context;

		public CartManagerController(ApplicationDbContext context)
		{
			_context = context;
		}

		public IActionResult Cart()
		{
			return View();
		}

		public JsonResult getCartSummary()
		{
			var cart = new CartManager(_context).GetCart(this.HttpContext);
			return new JsonResult(cart);
		}
		public JsonResult getCartCount()
		{
			var count = new CartManager(_context).GetCount(this.HttpContext);
			return new JsonResult(
				new GeneralJsonResponses()
				{
					success = true,
					Message = new List<string>()
					{
						count.ToString()
					},
					StatusCode =200
					
				}) ; 
		}
		[HttpPost]
		public JsonResult AddToCart(int id)
		{
			// Retrieve the product from the database
			var addedproduct = _context.Product
				.FirstOrDefault(product => product.Id == id);

			// Add it to the shopping cart
			var cart = new CartManager(_context).GetCart(this.HttpContext);

			cart.AddToCart(addedproduct);

			return new JsonResult(new GeneralJsonResponses()
			{
				success = true,
				Message = new List<string>()
				{
					addedproduct.Name +" Successfully Added to cart"
				},
				StatusCode = 200
			});

		}
		public JsonResult RemoveFromCart(int id)
		{
			//Retrieve the product from the database
			var addedproduct = _context.Product
				.Single(product => product.Id == id);

			// Add it to the shopping cart
			var cart = new CartManager(_context).GetCart(this.HttpContext);

			cart.RemoveFromCartCompeletely(id);

			return new JsonResult(new GeneralJsonResponses()
			{
				success = true,
				Message = new List<string>()
				{
					addedproduct.Name + " Successfully removed from cart"
				},
				StatusCode = 200
			});

		}

        //
        public JsonResult UpdateCartCountCart(int id,int count)
        {
            //Retrieve the product from the database
            var addedproduct = _context.Product
                .Single(product => product.Id == id);

            // get isnatcne of shopping cart
            var cart = new CartManager(_context).GetCart(this.HttpContext);

            cart.updateCartCountCart(id,count);

            return new JsonResult(new GeneralJsonResponses()
            {
                success = true,
                Message = new List<string>()
                {
                    addedproduct.Name + " Successfully Updated"
                },
                StatusCode = 200
            });

        }
        public JsonResult getCartFullDetails()
        {
            var cart = new CartManager(_context).GetCart(this.HttpContext);
			if(cart.GetCount()>0)
			{
				JsonObject details = new JsonObject();
                details.Add("CartId", cart.GetCartId(this.HttpContext));
                details.Add("Count", cart.GetCount());
				details.Add("Total", cart.GetTotal());
				var cartItems = cart.GetCartItems();
				JsonArray cartFullDetailedItems = new JsonArray();
				foreach(var item in cartItems)
				{
					var productId = item.ProductId;
					var product = _context.Product.Where(product => product.Id == productId).FirstOrDefault();
					product.Quanity = item.count;
					cartFullDetailedItems.Add(product);

                }
				details.Add("Items",cartFullDetailedItems);

				return new JsonResult(new GeneralJsonResponses()
				{
					success=true,
					Result= details,
					StatusCode= 200,
					Message= new List<string>() { "Cart has items "}

				});
			}

            return new JsonResult(new GeneralJsonResponses()
			{
				success=false,
				Message = new List<string>() { "No data found"},
			    StatusCode = 200
			});
        }

    }
}
