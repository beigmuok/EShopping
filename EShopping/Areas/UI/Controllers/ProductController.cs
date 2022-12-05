using EShopping.Data;
using EShopping.Models;
using EShopping.Utility;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShopping.Areas.UI.Controllers
{
	[Area("UI")]
	public class ProductController : Controller
	{
		public readonly ApplicationDbContext _context;
		public readonly ILogger<ProductController> _logger;

		public ProductController(ILogger<ProductController> logger, ApplicationDbContext context)
		{
			_context = context;
			_logger = logger;
		}
		public IActionResult Index()
		{
			return View(_context.Product.Include(c => c.ProductTag).Include(m => m.ProductType).ToList());
		}
		[HttpPost]
        public JsonResult filterByCost(decimal lowerAmountInput, decimal highAmountInput)
        {
			var result = _context.Product.Include(c => c.ProductTag).Include(m => m.ProductType).Where(m => m.Price >= lowerAmountInput && m.Price <= highAmountInput).ToList();
			if (result.Count == 0)
			{
                return new JsonResult(null);
            }

			return new JsonResult(result);

			

        }
        //go to details page
        public IActionResult Detail(int id)
		{
			if (id == null)
				return NotFound();
			var product = _context.Product.Include(c => c.ProductType).Include(m => m.ProductTag).FirstOrDefault(v => v.Id == id);
			if (product == null)
				return NotFound();
			return View(product);

		}
		//Method to add to the from details page cart
		[HttpPost]
		[ActionName("Detail")]
		public ActionResult ProductDetail(int id)
		{
			List<Product> products = new List<Product>();
			if(id==null)
				return NotFound();
			var product = _context.Product.Include(n => n.ProductType).Include(m => m.ProductTag).FirstOrDefault(c => c.Id == id);
			if(product == null)
				return NotFound();
			products = HttpContext.Session.Get<List<Product>>("products");
			if(products == null)
                products= new List<Product>();  

			//find out if the product exist in the cart so that only quanity is changed

			products.Add(product);
			HttpContext.Session.Set("products", products);

            return RedirectToAction(nameof(Index));
		}
		//method to remove product from cart
		[ActionName("Remove")]
		public IActionResult RemoveFromCart(int id)
		{
			List<Product> products = HttpContext.Session.Get < List <Product >> ("products");
			if (products != null)
			{
				var product = products.FirstOrDefault(c => c.Id == id);
				if (product != null)
				{
					products.Remove(product);
					HttpContext.Session.Set("products", products);
				}
			}

			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
        public IActionResult Remove(int id)
        {
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
            if (products != null)
            {
                var product = products.FirstOrDefault(c => c.Id == id);
                if (product != null)
                {
                    products.Remove(product);
                    HttpContext.Session.Set("products", products);
                }
            }

            return RedirectToAction(nameof(Index));
        }

		public IActionResult Cart()
		{
            List<Product> products = HttpContext.Session.Get<List<Product>>("products");
			if(products == null)
			{
				products = new List<Product>();
			}
			return View(products);
		}
    }
}
