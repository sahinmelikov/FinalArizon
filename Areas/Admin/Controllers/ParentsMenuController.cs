using FinalArizon.DAL;
using FinalArizon.Models;
using FinalArizon.Utilities.Extensions;
using FinalArizon.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Diagnostics.Metrics;

namespace FinalArizon.Areas.Admin.Controllers
{   
    [Area("Admin")]
    [Authorize(Roles = "Admin,Moderator")]
    public class ParentsMenuController : Controller
    {
     
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ParentsMenuController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            List<ParentsCategory> parentscategory = await _context.ParentsCategories.OrderByDescending(d => d.Id).ToListAsync();

            return View(parentscategory);

        }




        [HttpGet]
        public IActionResult Delete(int Id)
        {
            ParentsCategory? parents = _context.ParentsCategories?.Find(Id);

            if (parents == null)
            {
                return NotFound();

            }
            string FilePath = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img", parents.ImagePath);
            if (System.IO.File.Exists(FilePath))
            {
                System.IO.File.Delete(FilePath);
            }
            _context.ParentsCategories.Remove(parents);
            _context.SaveChanges();


            return RedirectToAction(nameof(Index));
        }






        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ParentsMenuCreateVM parentsmenu)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            if (!parentsmenu.Photo.CheckContentType("image/"))
            {
                ModelState.AddModelError("Photo", $"{parentsmenu.Photo.FileName} Sekil Tipinde olmalidir ");
                return View();
            }
            if (!parentsmenu.Photo.CheckFileSize(1500))
            {
                ModelState.AddModelError("Photo", $"{parentsmenu.Photo.FileName} - 200kb dan Cox Olmaz");
                return View();
            }

            string root = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img");
            string fileName = await parentsmenu.Photo.SaveAsync(root);
          

            ParentsCategory parents = new ParentsCategory()
            {
                Name = parentsmenu.Name,
                ImagePath = fileName,


            };
            await _context.ParentsCategories.AddAsync(parents);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        public async Task<IActionResult> Update(int id)
        {
            //ServiceUpdateVM member = await _context.Services.FindAsync(id);
            //if (member == null) return NotFound();

            return View(new ParentsMenuUpdateVM());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(ParentsMenuUpdateVM parentsmenuupdateVM)
        {
            if (!ModelState.IsValid) { return View(parentsmenuupdateVM); }

            if (!parentsmenuupdateVM.Photo.ContentType.Contains("image/"))
            {
                ModelState.AddModelError("Photo", $"{parentsmenuupdateVM.Photo.FileName} Sekil Tipinde olmalidir ");
                return View();
            }
            if (!parentsmenuupdateVM.Photo.CheckFileSize(1800))
            {
                ModelState.AddModelError("Photo", $"{parentsmenuupdateVM.Photo.FileName} - 200kb dan Cox Olmaz");
                return View();
            }
            ParentsCategory? product = (await _context.ParentsCategories.FindAsync(parentsmenuupdateVM.Id));

            if (product == null) { return NotFound(); }

            string rootPath = Path.Combine(_webHostEnvironment.WebRootPath, "RootAllPictures", "img");
            string Oldfilename = Path.Combine(rootPath, product.ImagePath);

            if (System.IO.File.Exists(Oldfilename))

            { System.IO.File.Delete(Oldfilename); }

            string Newfilename = Guid.NewGuid().ToString() + parentsmenuupdateVM.Photo.FileName;
            string resultpath = Path.Combine(rootPath, Newfilename);


          

            using (FileStream fileStream = new(resultpath, FileMode.Create))
            {
                await parentsmenuupdateVM.Photo.CopyToAsync(fileStream);
            }
           
            product.Name = parentsmenuupdateVM.Name;
           

            product.ImagePath = Newfilename;
          
           
           
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));



        }

    }


}
