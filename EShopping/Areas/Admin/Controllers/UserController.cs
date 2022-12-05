using EShopping.Data;
using EShopping.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EShopping.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context= context;
        }


        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var userList = _context.ApplicationUser.Include(u => u.company).ToList();
            var userRole = _context.UserRoles.ToList();
            var roles = _context.Roles.ToList();

            foreach (var user in userList)
            {
                var roleId = userRole.FirstOrDefault(u => u.UserId == user.Id).RoleId;
                user.Role = roles.FirstOrDefault(u => u.Id == roleId).Name;
                if (user.company == null)
                {
                    user.company = new Company()
                    {
                        Name = ""
                    };
                }

            }
            return Json(new { data = userList });
        }
    }
}
