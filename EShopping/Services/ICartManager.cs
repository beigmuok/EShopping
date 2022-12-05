using EShopping.Areas.UI.Controllers;
using EShopping.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace EShopping.Services
{
	public interface ICartManager
	{

		//AddToCart takes an Album as a parameter and adds it to the user's cart.
		//Since the Cart table tracks quantity for each product,
		//it includes logic to create a new row if needed or just increment the quantity if the user has already ordered one copy of the product.
		public void AddToCart(Product product);

		//RemoveFromCart takes an Album ID and removes it from the user's cart. If the user only had one copy of the album in their cart, the row is removed.
		public int RemoveFromCart(int id);

        public void updateCartCountCart(int id,int count);
        //EmptyCart removes all items from a user's shopping cart.

        //EmptyCart removes all items from a user's shopping cart.
        public void EmptyCart();

		//GetCartItems retrieves a list of CartItems for display or processing.
		public List<Cart> GetCartItems();
		//   GetCount retrieves a the total number of albums a user has in their shopping cart.
		public int GetCount();
		//GetTotal calculates the total cost of all items in the cart.
		public decimal GetTotal();
		//CreateOrder converts the shopping cart to an order during the checkout phase.
		public int CreateOrder(Order order);
		//GetCart is a static method which allows our controllers to obtain a cart object. It uses the GetCartId method to handle reading the CartId from the user's session. The GetCartId method requires the HttpContextBase so that it can read the user's CartId from user's session.
		public string GetCartId(HttpContext context);
		 
	}
}
