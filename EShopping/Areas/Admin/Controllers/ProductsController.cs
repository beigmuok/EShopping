using EShopping.Data;
using EShopping.HelperClasses;
using EShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace EShopping.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class ProductsController : Controller
	{
		public readonly ApplicationDbContext _context;
		public readonly IHostingEnvironment _he;
		//Provides information about the web hosting environment an application is running in

		public ProductsController(ApplicationDbContext context, IHostingEnvironment he)
		{
			_context = context;
			_he = he;
		}
		public IActionResult Index()
		{
			return View(_context.Product.Include(c => c.ProductTag).Include(t => t.ProductType).ToList());
		}


		public IActionResult Create()
		{
			ViewData["ProductTypeId"] = new SelectList(_context.ProductType.ToList(), "Id", "TypeName");
			ViewData["ProductTagId"] = new SelectList(_context.ProductTag.ToList(), "Id", "Name");
			return View();
		}
		[HttpPost]
		public async Task<ActionResult> Create(Product product, IFormFile image)
		{
			ModelState.Remove("Image");
			ViewData["ProductTypeId"] = new SelectList(_context.ProductType.ToList(), "Id", "TypeName");
			ViewData["ProductTagId"] = new SelectList(_context.ProductTag.ToList(), "Id", "Name");
			if (ModelState.IsValid)
			{
				var productExists = _context.Product.FirstOrDefault(c => c.Name == product.Name);
				if (productExists != null)
				{
					ModelState.AddModelError("Name", "Product with  same Name " + product.Name + " already Exists");
					ViewBag.message = "Product with  same Name " + product.Name + " already Exists";
					ViewData["ProductTypeId"] = new SelectList(_context.ProductType.ToList(), "Id", "TypeName");
					ViewData["ProductTagId"] = new SelectList(_context.ProductTag.ToList(), "Id", "Name");
					return View(product);
				}


				//let handle the image
				if(image != null)
				{
					var name = Path.Combine(_he.WebRootPath + "/Images",Path.GetFileName(image.FileName));
					await image.CopyToAsync(new FileStream(name, FileMode.Create));
					product.Image = "Images/" + image.FileName;
				}
				//if image is missing, use noimage image
				if(image== null)
				{
					product.Image = "Images/noimage.png";
				}

				//now lets add the product

				_context.Product.Add(product);
				await _context.SaveChangesAsync();
				TempData["Save"] = JsonConvert.SerializeObject(new ResponsesFromModels(0, "Product Created Successfully "));
				return RedirectToAction(nameof(Index));


			}

			return View(product);

		}

		//private IEnumerable<SelectListItem> GetProductTypes()
		//{
		//	return _context.ProductType
		//			 .Select(s => new SelectListItem
		//			 {
		//				 Value = s.Id.ToString(),
		//				 Text = s.TypeName
		//			 }).ToList();
		//}
		public IActionResult Edit(int id)
		{
			//view data uses the virtual productTypeId
			ViewData["ProductTypeId"] = new SelectList(_context.ProductType.ToList(), "Id", "TypeName");
			ViewData["ProductTagId"] = new SelectList(_context.ProductTag.ToList(), "Id", "Name");
			if(id ==null)
			{
				return NotFound();
			}
			var product = _context.Product.Include(c=> c.ProductTag).Include(m => m.ProductType).FirstOrDefault(v => v.Id == id);
			//product.AllProductTypes = GetProductTypes();
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}
		[HttpPost]
		public async Task<ActionResult> Edit(Product product, IFormFile image)
		{
			//ModelState.Remove("Image");
			if (ModelState.IsValid)
			{
				var productExists = _context.Product.AsNoTracking().FirstOrDefault(c => c.Name == product.Name);
				
				//let handle the image
				if (image != null)
				{
					var name = Path.Combine(_he.WebRootPath + "/Images", Path.GetFileName(image.FileName));
					await image.CopyToAsync(new FileStream(name, FileMode.Create));
					product.Image = "Images/" + image.FileName;
				}
				//if image is missing, use noimage image
				if (image == null)
				{
					product.Image = "Images/noimage.png";
				}
				ViewBag.ImageUrl = product.Image;

				//now lets add the product

				_context.Product.Update(product);
				await _context.SaveChangesAsync();
				TempData["Edit"] = JsonConvert.SerializeObject(new ResponsesFromModels(0, "Product updated Successfully "));
				return RedirectToAction(nameof(Index));


			}

			return View(product);

		}

		public ActionResult Detail(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			 var product = _context.Product.Include(c => c.ProductTag).Include(m => m.ProductType).FirstOrDefault(v => v.Id == id);
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}
		public ActionResult Delete(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var product = _context.Product.Include(c => c.ProductTag).Include(m => m.ProductType).Where(v => v.Id == id).FirstOrDefault();
			if (product == null)
			{
				return NotFound();
			}

			return View(product);
		}
		[HttpPost]
		[ActionName("Delete")]
		public async Task<ActionResult> DeleteConfirm(int id)
		{
			if (id == null)
			{
				return NotFound();
			}
			var product = _context.Product.AsNoTracking().FirstOrDefault(v => v.Id == id);
			if (product == null)
			{
				return NotFound();
			}
			_context.Remove(product);
			await _context.SaveChangesAsync();
			TempData["Delete"] = JsonConvert.SerializeObject(new ResponsesFromModels(0, "Product deleted Successfully "));

			return RedirectToAction(nameof(Index));	 
		}
	}


}


