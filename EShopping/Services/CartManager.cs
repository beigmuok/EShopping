using EShopping.Areas.UI.Controllers;
using EShopping.Data;
using EShopping.Models;
using EShopping.Utility;
using Microsoft.AspNetCore.Mvc;
using System.Web;


namespace EShopping.Services
{
	public class CartManager : ICartManager
	{
		public  readonly ApplicationDbContext _context;
		string ShoppingCartId { get; set; }
		public const string CartSessionKey = "CartId";
		
		public  CartManager(ApplicationDbContext Acontext)
		{
			_context = Acontext;
		}

		public   CartManager GetCart(HttpContext context)
		{
			var cart = new CartManager(_context);
			cart.ShoppingCartId = cart.GetCartId(context);
			return cart;
		}
		// Helper method to simplify shopping cart calls
		public  CartManager GetCart(Controller controller)
		{
			return GetCart(controller.HttpContext);
		}


		public void AddToCart(Product product)
		{
			//check if there is matching product in your cart(database)
			 var existingCartProduct =  _context.Cart.Where(c=> c.CartId == ShoppingCartId &&  c.ProductId == product.Id).FirstOrDefault();
			 if(existingCartProduct == null)
				{
				//prepare cart to add to the database
					Cart cart = new Cart()
					{
						CartId = ShoppingCartId,
						count = 1,
						ProductId = product.Id,
						DateCreated = DateTime.Now,
					};
					_context.Cart.Add(cart);
				}
			    else
				{
				    //add one count to the existing count
					existingCartProduct.count++;
				}

			 _context.SaveChanges();
		}

		
		public void EmptyCart()
		{
			//get list of cart associated with this shopping id
			var cartProducts = _context.Cart.Where(cart => cart.CartId == ShoppingCartId);
			if(cartProducts.Any())
			{
				foreach(var item in cartProducts)
				{
					_context.Cart.Remove(item);
				}
				_context.SaveChanges();
			}
		}

		public string GetCartId(HttpContext context)
		{
			if (context.Session.GetString(CartSessionKey) == null)
			{
				//if 
				if (!string.IsNullOrWhiteSpace(context.User.Identity.Name))
				{
					context.Session.SetString(CartSessionKey, context.User.Identity.Name);
						
				}
				else
				{
					// Generate a new random GUID using System.Guid class
					Guid tempCartId = Guid.NewGuid();
					// Send tempCartId back to client as a cookie
					context.Session.SetString(CartSessionKey,tempCartId.ToString());
				}
			}
			return context.Session.GetString(CartSessionKey);
		}

		public List<Cart> GetCartItems()
		{
			 return _context.Cart.Where(n=> n.CartId== ShoppingCartId).ToList();

		}

		public int GetCount()
		{
			// Get the count of each item in the cart and sum them up
			int? count = (from cartItems in _context.Cart
						  where cartItems.CartId == ShoppingCartId
						  select (int?)cartItems.count).Sum();
			// Return 0 if all entries are null
			return count ?? 0;
		}
		public int GetCount(HttpContext context)
		{
			string shoppingId = GetCartId(context);
			// Get the count of each item in the cart and sum them up
			var count =  _context.Cart.Where( n => n.CartId == shoppingId).ToList();
			// Return 0 if all entries are null
			return count.Count;
		}


		public decimal GetTotal()
		{
			// Multiply album price by count of that album to get 
			// the current price for each of those albums in the cart
			// sum all album price totals to get the cart total
			decimal? total = (from cartItems in _context.Cart
							  where cartItems.CartId == ShoppingCartId
							  select (int?)cartItems.count *
							  cartItems.Product.Price).Sum();

			return total ?? decimal.Zero;
		}

		public int RemoveFromCart(int id)
		{
			//// Get the cart
			var cartItem = _context.Cart.FirstOrDefault(
				cart => cart.CartId == ShoppingCartId
				&& cart.ProductId == id);

			int itemCount = 0;

			if (cartItem != null)
			{
				if (cartItem.count > 1)
				{
					cartItem.count--;
					itemCount = cartItem.count;
				}
				else
				{
					_context.Cart.Remove(cartItem);
				}
				// Save changes
				_context.SaveChanges();
			}
			return itemCount;

		}
        public int RemoveFromCartCompeletely(int id)
        {
            //// Get the cart
            var cartItem = _context.Cart.FirstOrDefault(
                cart => cart.CartId == ShoppingCartId
                && cart.ProductId == id);

            int itemCount = 0;

            if (cartItem != null)
            {
                
                    _context.Cart.Remove(cartItem);
              
                // Save changes
                _context.SaveChanges();
            }
            return itemCount;

        }
        // When a user has logged in, migrate their shopping cart to
        // be associated with their username
        public void MigrateCart(string userName)
		{
			var shoppingCart = _context.Cart.Where(
				c => c.CartId == ShoppingCartId);

			foreach (Cart item in shoppingCart)
			{
				item.CartId = userName;
			}
			_context.SaveChanges();
		}
		public int CreateOrder(Order order)
		{
            decimal orderTotal = 0;

            var cartItems = GetCartItems();
            // Iterate over the items in the cart, 
            // adding the order details for each
            foreach (var item in cartItems)
            {
                var orderDetail = new OrderDetails
                {
                    ProductId = item.ProductId,
                    OrderId = order.OrderId,
                    UnitPrice = item.Product.Price,
                    Quantity = item.count
                };
                // Set the order total of the shopping cart
                orderTotal += (item.count * item.Product.Price);

                _context.OrderDetails.Add(orderDetail);

            }
            // Set the order's total to the orderTotal count
            order.Total = orderTotal;

            // Save the order
            _context.SaveChanges();
            // Empty the shopping cart
            EmptyCart();
            // Return the OrderId as the confirmation number
            return order.OrderId;
        }

        public void updateCartCountCart(int id, int count)
        {
            var cartItem = _context.Cart.FirstOrDefault( cart => cart.CartId == ShoppingCartId && cart.ProductId == id);

            if (cartItem != null)
            {               
			     cartItem.count = count;
                 _context.Cart.Update(cartItem);                
                // Save changes
                _context.SaveChanges();
            }
        }
    }
}
