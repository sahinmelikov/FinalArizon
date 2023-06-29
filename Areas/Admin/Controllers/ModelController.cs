using FinalArizon.DAL;
using FinalArizon.Models;
using FinalArizon.Utilities.Extensions;
using FinalArizon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;

namespace FinalArizon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ModelController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ModelController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Model> models = await _context.Models.OrderByDescending(d => d.Id).ToListAsync();
            return View(models);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Model model = _context.Models.Find(id);

            if (model == null)
            {
                return NotFound();
            }

            _context.Models.Remove(model);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            List<ParentsCategory> products = _context.ParentsCategories.ToList();
            List<SelectListItem> productItems = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            ViewBag.Products = productItems;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ModelCreateViewModel member)
        {
            if (!ModelState.IsValid)
            {
                List<ParentsCategory> parents = _context.ParentsCategories.ToList();
                List<SelectListItem> parentsItems = parents.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                ViewBag.Products = parentsItems;
                return View(member);
            }
            Model model = new Model()
            {
                Name = member.Name,
                ParentsCategoryId = member.ParentsCategoryId
            };

            await _context.Models.AddAsync(model);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int id)
        {
            Model model = await _context.Models.FindAsync(id);
            if (model == null)
            {
                return NotFound();
            }

            List<ParentsCategory> products = await _context.ParentsCategories.ToListAsync();
            List<SelectListItem> productItems = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            ViewBag.Products = productItems;

            ModelUpdateVM modelUpdateVM = new ModelUpdateVM()
            {
                Id = model.Id,
                Name = model.Name,
                ParentsCategoryId = model.ParentsCategoryId,
               
            };

            return View("Update", modelUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ModelUpdateVM modelUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                List<ParentsCategory> parents = await _context.ParentsCategories
                .ToListAsync();

                List<SelectListItem> productItems = parents.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                ViewBag.Products = productItems;

                return View(modelUpdateVM);
            }

            Model model = await _context.Models.FindAsync(modelUpdateVM.Id);
            if (model == null)
            {
                return NotFound();
            }

            model.Name = modelUpdateVM.Name;
            model.ParentsCategoryId = modelUpdateVM.ParentsCategoryId;


            // Burada ProductId değerinin geçerli bir ürün ID'si olduğunu kontrol ediyoruz
            ParentsCategory relatedProduct = await _context.ParentsCategories.FindAsync(modelUpdateVM.ParentsCategoryId);
            if (relatedProduct == null)
            {
                ModelState.AddModelError("ProductId", "Özür dileriz, böyle bir ürün bulunamadı.");

                List<ParentsCategory> updatedProducts = await _context.ParentsCategories.ToListAsync();
                List<SelectListItem> updatedProductItems = updatedProducts.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                ViewBag.Products = updatedProductItems;

                return View(modelUpdateVM);
            }

            model.ParentsCategoryId = modelUpdateVM.ParentsCategoryId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

    }
   
}
