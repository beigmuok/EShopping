using EShopping.Data;
using EShopping.HelperClasses;
using EShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace EShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTagsController : Controller
    {
        public readonly ApplicationDbContext _context;
        public ProductTagsController(ApplicationDbContext context)
        {
                _context = context;
        }
        public IActionResult Index()
        {
            return View(_context.ProductTag.ToList());
        }
        public IActionResult Create() 
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductTag productTag) {

            var tag = await _context.ProductTag.FirstOrDefaultAsync(x => x.Name== productTag.Name);
            if (tag != null)
            {
                ModelState.AddModelError("Name", "The tag " + productTag.Name + " already exists");
				//TempData["Save"] = JsonConvert.SerializeObject(new ResponsesFromModels(1, "The tag " + productTag.Name + " already exists"));
			}
            if(ModelState.IsValid)
            {
                _context.ProductTag.Add(productTag);
                await _context.SaveChangesAsync();
                TempData["Save"] = JsonConvert.SerializeObject(new ResponsesFromModels(0, "Product Tag Created Successfully "));
				return RedirectToAction(nameof(Index));
            }
   //         TempData.Clear();
			//TempData["Save"] = JsonConvert.SerializeObject(new ResponsesFromModels(1, "Product Tag Not Created "));
			return View();
            
        }

        public IActionResult Edit(int id)
        {
            if(id==null)
                return NotFound();
            var tag = _context.ProductTag.Find(id);
            if (tag==null)
                return NotFound();

               return View(tag);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProductTag tag)
        {  
            if (ModelState.IsValid)
            {
                _context.ProductTag.Update(tag);
                await _context.SaveChangesAsync();

                TempData["Edit"] = JsonConvert.SerializeObject(new ResponsesFromModels(0, "Success in Editing Product Tag"));
            }
            return RedirectToAction(nameof(Index));

        }
        public IActionResult Detail(int? id)
        {
			if (id == null)
				return NotFound();
			var tag = _context.ProductTag.Find(id);
			if (tag == null)
				return NotFound();
			return View(tag);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detail(ProductTag tag)
        {
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
			if (id == null)
				return NotFound();
			var tag = _context.ProductTag.Find(id);
			if (tag == null)
				return NotFound();

			return View(tag);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
		public async Task<ActionResult> Delete(int? id, ProductTag tag)
		{
			if (id == null)
				return NotFound();
			if (tag.Id != id)
				return NotFound();
			tag = _context.ProductTag.Find(id);
			if (tag == null)
				return NotFound();
			if (ModelState.IsValid)
			{
				_context.Remove(tag);
				await _context.SaveChangesAsync();
				TempData["Delete"] = JsonConvert.SerializeObject(new ResponsesFromModels(0, "Product Tag has been deleted"));
				return RedirectToAction(nameof(Index));
			}
			return View(tag);
		}
	}
}
