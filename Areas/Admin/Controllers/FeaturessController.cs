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
using System.Linq;
using System.Threading.Tasks;

namespace FinalArizon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class FeaturressController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FeaturressController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Feature> features = await _context.Features.OrderByDescending(d => d.Id).ToListAsync();
            return View(features);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Feature feature = _context.Features.Find(id);

            if (feature == null)
            {
                return NotFound();
            }

            _context.Features.Remove(feature);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            List<Product> products = _context.Products.ToList();
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
        public async Task<IActionResult> Create(FeatureCreateVM member)
        {
            if (!ModelState.IsValid)
            {
                List<Product> products = await _context.Products.ToListAsync();
                List<SelectListItem> productItems = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                ViewBag.Products = productItems;

                return View(member);
            }

            Feature feature = new Feature()
            {
                AboutProduct = member.AboutProduct,
                BrandName = member.BrandName,
                ModelName = member.ModelName,
                Vol = member.Vol,
                Tension = member.Tension,
                Size = member.Size,
                Weight = member.Weight,
                ProductId = member.ProductId
            };

            await _context.Features.AddAsync(feature);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            Feature feature = await _context.Features.FindAsync(id);
            if (feature == null)
            {
                return NotFound();
            }

            List<Product> products = await _context.Products.ToListAsync();
            List<SelectListItem> productItems = products.Select(p => new SelectListItem
            {
                Value = p.Id.ToString(),
                Text = p.Name
            }).ToList();

            ViewBag.Products = productItems;

            FeatureUpdateVM featureUpdateVM = new FeatureUpdateVM()
            {
                Id = feature.Id,
                AboutProduct = feature.AboutProduct,
                BrandName = feature.BrandName,
                ModelName = feature.ModelName,
                Vol = feature.Vol,
                Tension = feature.Tension,
                Size = feature.Size,
                Weight = feature.Weight,
                ProductId = feature.ProductId
            };

            return View("Update", featureUpdateVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(FeatureUpdateVM featureUpdateVM)
        {
            if (!ModelState.IsValid)
            {
                List<Product> products = await _context.Products
                .ToListAsync();

                List<SelectListItem> productItems = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                ViewBag.Products = productItems;

                return View(featureUpdateVM);
            }

            Feature feature = await _context.Features.FindAsync(featureUpdateVM.Id);
            if (feature == null)
            {
                return NotFound();
            }

            feature.AboutProduct = featureUpdateVM.AboutProduct;
            feature.BrandName = featureUpdateVM.BrandName;
            feature.ModelName = featureUpdateVM.ModelName;
            feature.Vol = featureUpdateVM.Vol;
            feature.Tension = featureUpdateVM.Tension;
            feature.Size = featureUpdateVM.Size;
            feature.Weight = featureUpdateVM.Weight;

            // Burada ProductId değerinin geçerli bir ürün ID'si olduğunu kontrol ediyoruz
            Product relatedProduct = await _context.Products.FindAsync(featureUpdateVM.ProductId);
            if (relatedProduct == null)
            {
                ModelState.AddModelError("ProductId", "Özür dileriz, böyle bir ürün bulunamadı.");

                List<Product> updatedProducts = await _context.Products.ToListAsync();
                List<SelectListItem> updatedProductItems = updatedProducts.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                ViewBag.Products = updatedProductItems;

                return View(featureUpdateVM);
            }

            feature.ProductId = featureUpdateVM.ProductId;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}