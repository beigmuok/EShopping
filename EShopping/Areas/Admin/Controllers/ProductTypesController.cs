using EShopping.Data;
using EShopping.HelperClasses;
using EShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace EShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductTypesController : Controller
	{
       
        public readonly ApplicationDbContext _context;
        public ProductTypesController(ApplicationDbContext context)
        {
            _context = context;
        }
       
        public IActionResult Index()
        {
            return View(_context.ProductType.ToList());
        }
        //is invocked when the Create view is called.
        //it simply returns the view
        public IActionResult Create()
        {
            return View();
        }
        //is invocked when thein the form to create a product, the post form in the create view
        //if creation successful, its redirected to index view, attached with it temp data to show it was a success
        //otherwiwise, it remains in the create view with details of the product
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.ProductType.Add(productType);
                await _context.SaveChangesAsync();
                TempData["Save"] = JsonConvert.SerializeObject( new ResponsesFromModels(0, "Product Type Created Successfully")); 
                return RedirectToAction(actionName: nameof(Index));
            }
			TempData["Save"] = JsonConvert.SerializeObject(new ResponsesFromModels(1, "Product Type Not Created"));
			return View(productType);
        }

        public ActionResult Edit(int id)
        {
            if (id == null)
                return NotFound();
            var productType = _context.ProductType.Find(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ProductType productType)
        {
            if (ModelState.IsValid)
            {
                _context.Update(productType);
                await _context.SaveChangesAsync();
                TempData["Edit"] = "Product type has been updated";
                return RedirectToAction(actionName: nameof(Index));
            }
            return View(productType);
        }

        public ActionResult Detail(int? id)
        {
            if (id == null)
                return NotFound();
            var productType = _context.ProductType.Find(id);
            if (productType == null)
                return NotFound();
            return View(productType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Detail(ProductType productType)
        {
            return RedirectToAction(nameof(Index));
        }

        public ActionResult Delete(int id)
        {
            if (id == null)
                return NotFound();
            var productType = _context.ProductType.Find(id);
            if (productType == null)
                return NotFound();
            return View(productType);

        }

        //if we want to go ahead and delete it, then we use
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int? id, ProductType productType)
        {
            if (id == null)
                return NotFound();
            if (productType.Id != id)
                return NotFound();
            productType = _context.ProductType.Find(id);
            if (productType == null)
                return NotFound();
            if (ModelState.IsValid)
            {
                _context.Remove(productType);
                await _context.SaveChangesAsync();
                TempData["Delete"] = JsonConvert.SerializeObject(new ResponsesFromModels(0, "Product Type has been deleted")); 
                return RedirectToAction(nameof(Index));
            }
            return View(productType);
        }
    }
}
