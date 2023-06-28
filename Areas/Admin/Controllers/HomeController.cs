using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace FinalArizon.Areas.Admin.Controllers
{
   
        [Authorize(Roles = "Admin,Maderator")]
        public class HomeController : Controller
        {
            [Area("Admin")]
            public IActionResult Index()
            {
                return View();
            }
        }
   
}
