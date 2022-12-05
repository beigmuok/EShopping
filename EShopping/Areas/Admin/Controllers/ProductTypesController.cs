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
        [HttpPost]
        public JsonResult getAllTypes()
        {
            return new JsonResult(_context.ProductType.ToList());
        }

		public JsonResult SaveOrUpdate(ProductType productType)
		{

             if (productType == null)
                return new JsonResult(new GeneralJsonResponses()
                {
                    success=false,
                    Message = new List<string>()
                    {
                        "Something went Wrong, The data passed is invalid"
                    },
                    StatusCode= 400,
                });
                if (productType.Id == 0) //making a new Entry
                {
                    //check for productType with same name
                    var productTypeExists = _context.ProductType.Where(n => n.TypeName == productType.TypeName).FirstOrDefault();
                    if (productTypeExists != null)
                        return new JsonResult(new GeneralJsonResponses()
                        {
                            success = false,
                            Message = new List<string>()
                        {
                            "The product Type with the name " + productType.TypeName +" already Exists"
                        },
                            StatusCode = 400,
                        });

                    _context.ProductType.Add(productType);
                    _context.SaveChanges();

                    return new JsonResult(new GeneralJsonResponses()
                    {
                        success = true,
                        Message = new List<string>()
                        {
                            "The product with Type the name " + productType.TypeName +" successfully Save"
                        },
                        StatusCode = 201,
                    });

                }else //update
                {
				    var productTypeExists = _context.ProductType.Where(n => n.TypeName == productType.TypeName && n.Id != productType.Id).FirstOrDefault();
				    if (productTypeExists != null)
					return new JsonResult(new GeneralJsonResponses()
					{
						success = false,
						Message = new List<string>()
						{
							"The product Type with the name " + productType.TypeName +" already Exists"
						},
						StatusCode = 400,
					});
                    //update now
                    ProductType toUpdate = _context.ProductType.Where(n=> n.Id == productType.Id).FirstOrDefault();
                    if (toUpdate != null)
                    {
                        toUpdate.TypeName = productType.TypeName;

                        _context.Update(toUpdate);

                        _context.SaveChanges();

                        return new JsonResult(new GeneralJsonResponses()
                        {
                            success = true,
                            Message = new List<string>()
                                        {
                                            "The product type updated successfully to " + productType.TypeName
                                        },
                            StatusCode = 201,
                        });
                    }

				return new JsonResult(new GeneralJsonResponses()
				{
					success = false,
					Message = new List<string>()
							{
								"The product type does not exist"
							},
					StatusCode = 400,
				});


			}
		}
        public JsonResult getTypeDetails(int id)
        {
            return new JsonResult(_context.ProductType.Where(n=> n.Id == id).ToList());
        }
		public JsonResult deleteType(int id)
		{
            if(id == 0)
            {
                return new JsonResult(new GeneralJsonResponses()
                {
                    success = false,
                    Message= new List<string>()
                    {
                        "Invalid Information provided"
                    },
                    StatusCode= 400,
                });
            }
            ProductType productType = _context.ProductType.Where(n=> n.Id==id).FirstOrDefault();
            if(productType == null)
            {
				return new JsonResult(new GeneralJsonResponses()
				{
					success = false,
					Message = new List<string>()
					{
						"No type for given ID"
					},
					StatusCode = 400,
				});
			}

            _context.Remove(productType);
            _context.SaveChanges();



			return new JsonResult(  (new GeneralJsonResponses()
				      {
					        success = true,
					        Message = new List<string>()
					        {
						        productType.TypeName + " successfully deleted"
					        },
					        StatusCode = 400,
				      })
                      );
            
		}
	}
}
