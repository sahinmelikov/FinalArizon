using FinalArizon.DAL;

using FinalArizon.Models;
using FinalArizon.Utilities.Extensions;
using FinalArizon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace FinalArizon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public async Task<IActionResult> Index()
        {
            List<Product> products = await _context.Products.OrderByDescending(d=>d.Id).ToListAsync();
        
            return View(products);
          
        }




        [HttpGet]
        public IActionResult Delete(int Id)
        {
            Product? product = _context.Products?.Find(Id);

            if (product == null)
            {
                return NotFound();

            }
            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img", product.ImagePath);
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
            _context.Products.Remove(product);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }






        public IActionResult Create()
        {
            List<Models.Model> models = _context.Models.Include(m => m.ParentsCategory).ToList();
            List<SelectListItem> modelItems = models.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = $"{m.Name} - {m.ParentsCategory.Name}"
            }).ToList();

            ViewBag.Products = modelItems;

            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductCreateVM member)
        {
            if (!ModelState.IsValid)
            {
                List<Product> products = await _context.Products.ToListAsync();
                List<SelectListItem> productItems = products.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                ViewBag.Product = productItems;

                return View(member);
            }


            if (!member.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{member.Photo.FileName} Şekil Tipinde Olmalıdır");
                ViewBag.Product = await _context.Products
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .ToListAsync();

                return View(member);
            }

            if (!member.Photo.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", $"{member.Photo.FileName} - 200kb'dan Fazla Olamaz");
                ViewBag.Product = await _context.Products
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .ToListAsync();

                return View(member);
            }

            string root = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img");
            string fileName = await member.Photo.SaveAsync(root);
            string fileNameColor = await member.ProductColorPhoto.SaveAsync(root);

            if (member.DiscountPrice > member.Price)
            {
                ModelState.AddModelError("DiscountPrice", "İndirimli fiyat, normal fiyattan büyük olamaz");
                ViewBag.Product = await _context.Products
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .ToListAsync();

                return View(member);
            }

            Product product = new Product()
            {
                Name = member.Name,
                ImagePath = fileName,
                Description = member.Description,
                Price = member.Price,
                DiscountPrice = member.DiscountPrice,
                ProductCode = member.ProductCode,
                Delivery = member.Delivery,
                Dimensions = member.Dimensions,
                ModelId = member.ModelId,
                ProductQuantity = member.ProductQuantity,
                ProductİmageColor = fileNameColor
            };

            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }






        public async Task<IActionResult> Update(int id)
        {
            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            List<Models.Model> models = _context.Models.Include(m => m.ParentsCategory).ToList();
            List<SelectListItem> modelItems = models.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = $"{m.Name} - {m.ParentsCategory.Name}"
            }).ToList();

            ViewBag.Products = modelItems;

            ProductUpdateVM updateVM = new ProductUpdateVM()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                Delivery = product.Delivery,
                Dimensions = product.Dimensions,
                ProductQuantity = product.ProductQuantity,
                ModelId = product.ModelId,
            };

            return View("Update", updateVM);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ProductUpdateVM updateVM)
        {
            if (!ModelState.IsValid)
            {
                List<Models.Model> models = await _context.Models.ToListAsync();
                List<SelectListItem> modelItems = models.Select(p => new SelectListItem
                {
                    Value = p.Id.ToString(),
                    Text = p.Name
                }).ToList();

                ViewBag.Models = modelItems;

                return View(updateVM);
            }

            if (!updateVM.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", $"{updateVM.Photo.FileName} Şekil Tipinde Olmalıdır");
                ViewBag.Models = await _context.Models
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .ToListAsync();

                return View(updateVM);
            }

            if (!updateVM.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photo", $"{updateVM.Photo.FileName} - 200kb'dan Fazla Olamaz");
                ViewBag.Models = await _context.Models
                    .Select(p => new SelectListItem
                    {
                        Value = p.Id.ToString(),
                        Text = p.Name
                    })
                    .ToListAsync();

                return View(updateVM);
            }

            Product product = await _context.Products.FindAsync(updateVM.Id);

            if (product == null)
            {
                return NotFound();
            }

            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img");
            string oldFilePath = Path.Combine(rootPath, product.ImagePath);

            if (System.IO.File.Exists(oldFilePath))
            {
                System.IO.File.Delete(oldFilePath);
            }

            string newFileName = Guid.NewGuid().ToString() + updateVM.Photo.FileName;
            string newFilePath = Path.Combine(rootPath, newFileName);

            using (FileStream fileStream = new FileStream(newFilePath, FileMode.Create))
            {
                await updateVM.Photo.CopyToAsync(fileStream);
            }

            string newColorFileName = Guid.NewGuid().ToString() + updateVM.ProductColorPhoto.FileName;
            string newColorFilePath = Path.Combine(rootPath, newColorFileName);

            using (FileStream fileStream = new FileStream(newColorFilePath, FileMode.Create))
            {
                await updateVM.ProductColorPhoto.CopyToAsync(fileStream);
            }

            if (updateVM.DiscountPrice >= updateVM.Price)
            {
                return NotFound("Bu Qiymet Ucuzlashmis Qiymetler Kategoriyasina Daxil Ola Bilmez");
            }

            product.Name = updateVM.Name;
            product.Description = updateVM.Description;
            product.Price = updateVM.Price;
            product.DiscountPrice = updateVM.DiscountPrice;
            product.ProductCode = updateVM.ProductCode;
            product.Delivery = updateVM.Delivery;
            product.Dimensions = updateVM.Dimensions;
            product.ProductQuantity = updateVM.ProductQuantity;
            product.ModelId = updateVM.ModelId;
            product.ImagePath = newFileName;
            product.ProductİmageColor = newColorFileName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }


    }
}
