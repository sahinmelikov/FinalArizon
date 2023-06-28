using FinalArizon.DAL;
using FinalArizon.Models;
using FinalArizon.Utilities.Extensions;
using FinalArizon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace FinalArizon.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public SliderController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<Slider> sliders = await _context.Sliders.OrderByDescending(d => d.Id).ToListAsync();
            return View(sliders);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            Slider slider = _context.Sliders.Find(id);

            if (slider == null)
            {
                return NotFound();
            }

            string filePath = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img", slider.ImagePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }

            _context.Sliders.Remove(slider);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Create()
        {
            SliderCreateVM model = new SliderCreateVM();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(SliderCreateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (!model.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{model.Photo.FileName} resim formatında olmalıdır.");
                return View(model);
            }

            if (!model.Photo.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", $"{model.Photo.FileName} - 200 KB'dan fazla olamaz.");
                return View(model);
            }

            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img");
            string fileName = await model.Photo.SaveAsync(rootPath);

            Slider slider = new Slider()
            {
                IsActive = model.Isactive,
                ImagePath = fileName
            };

            await _context.Sliders.AddAsync(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Update(int id)
        {
            Slider slider = await _context.Sliders.FindAsync(id);
            if (slider == null)
            {
                return NotFound();
            }

            SliderUpdateVM model = new SliderUpdateVM()
            {
                Id = slider.Id,
                Isactive = slider.IsActive
            };

            return View("Update", model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(SliderUpdateVM model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            Slider slider = await _context.Sliders.FindAsync(model.Id);
            if (slider == null)
            {
                return NotFound();
            }

            if (!model.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", $"{model.Photo.FileName} resim formatında olmalıdır.");
                return View(model);
            }

            if (!model.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photo", $"{model.Photo.FileName} - 200 KB'dan fazla olamaz.");
                return View(model);
            }

            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img");
            string oldFileName = Path.Combine(rootPath, slider.ImagePath);

            if (System.IO.File.Exists(oldFileName))
            {
                System.IO.File.Delete(oldFileName);
            }

            string newFileName = $"{Path.GetRandomFileName()}{Path.GetExtension(model.Photo.FileName)}";
            string filePath = Path.Combine(rootPath, newFileName);

            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Photo.CopyToAsync(fileStream);
            }

            slider.IsActive = model.Isactive;
            slider.ImagePath = newFileName;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

    }
}
