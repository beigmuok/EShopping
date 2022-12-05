using EShopping.Data;
using EShopping.Models;
using EShopping.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EShopping.Areas.UI.Controllers
{	[Area("UI")]
    [Authorize]
    public class CheckoutController : Controller
    {
        public readonly ApplicationDbContext _context;
        const string PromoCode = "FREE";
        public CheckoutController(ApplicationDbContext context)
        {
            _context= context;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddressAndPayment(IFormCollection values)
        {

            var order = new Order();
           
            //TryUpdateModel(order);
            try
            {
                if (string.Equals(values["PromoCode"], PromoCode,
                    StringComparison.OrdinalIgnoreCase) == false)
                {
                    return View(order);
                }
                else
                {
                    order.Username = User.Identity.Name;
                    order.OrderDate = DateTime.Now;

                    //Save Order
                    _context.Order.Add(order);
                    _context.SaveChanges();
                    //Process the order
                    var cart = new CartManager(_context).GetCart(this.HttpContext);
                    cart.CreateOrder(order);

                    return RedirectToAction("Complete",
                        new { id = order.OrderId });
                }
            }
            catch
            {
                //Invalid - redisplay with errors
                return View(order);
            }
        }
        //
        // GET: /Checkout/Complete
        public ActionResult Complete(int id)
        {
            // Validate customer owns this order
            bool isValid = _context.Order.Any(
                o => o.OrderId == id &&
                o.Username == User.Identity.Name);

            if (isValid)
            {
                return View(id);
            }
            else
            {
                return View("Error");
            }
        }

    }
}
