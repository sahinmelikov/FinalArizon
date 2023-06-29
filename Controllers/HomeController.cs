using FinalArizon.DAL;
using FinalArizon.Models;
using FinalArizon.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace FinalArizon.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _appDbContext;

        public HomeController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public IActionResult Index()
        {
            HomeVM homeVM = new HomeVM()
            {
                Products = _appDbContext.Products
                    .Include(p => p.Features)
                    .OrderByDescending(d=>d.Id)
                    .ToList(),

                Sliders = _appDbContext.Sliders.OrderByDescending(d => d.Id).ToList(),

                ParentsCategories = _appDbContext.ParentsCategories
                .Include(p => p.Features)
                .Include(p=>p.Models)
                .OrderByDescending(d => d.Id)
                .ToList()

               
        };
 ViewBag.dbServiceCount =  _appDbContext.Products.Count();

            return View(homeVM);
        }
        public async Task<IActionResult> LoadMore(int skip = 0, int take = 8)
        {

            //List<Service> services = await _context.Services
            //    .OrderByDescending(s => s.Id)
            //    .Where(s => !s.IsDeleted)
            //    .Skip(skip)
            //    .Take(take)
            //    .Include(s => s.ServiceImages)
            //    .ToListAsync();
            //return PartialView("_ServicesPartialView",services);
            //    HomeVM homeVM = new HomeVM()
            //    {
            //        Products = _appDbContext.Products
            //            .Include(p => p.Features)
            //            .OrderByDescending(d => d.Id)
            //            .ToList(),

            //        Sliders = _appDbContext.Sliders.OrderByDescending(d => d.Id).ToList(),

            //        ParentsCategories = _appDbContext.ParentsCategories
            //        .Include(p => p.Features)
            //        .Include(p => p.Models)
            //        .OrderByDescending(d => d.Id)
            //        .ToList()

            //    };
            //        return PartialView("_ServicesPartialView", homeVM);
            //}
            return ViewComponent("Home", new { skip = skip, take = take });
        }


            public IActionResult Category(int categoryId)
        {
            var products = _appDbContext.Products
                .Where(p => p.Model.ParentsCategoryId == categoryId)
                .OrderByDescending(d=>d.Id)
                .ToList();

            var homeVM = new HomeVM
            {
                Products = products,
                Sliders = _appDbContext.Sliders.OrderByDescending(d => d.Id).ToList(),

                ParentsCategories = _appDbContext.ParentsCategories
                           .Include(p => p.Features)
                           .Include(p => p.Models)
                           .OrderByDescending(d => d.Id)
                           .ToList()
            };
            return View(homeVM);
        }

        public IActionResult SubCategory(int modelId)
        {
          
            var products = _appDbContext.Products
                
                .OrderByDescending(d => d.Id)
                .Where(p => p.ModelId == modelId)
                .ToList();
            var homeVM = new HomeVM
            {
                Products = products,
                Sliders = _appDbContext.Sliders.OrderByDescending(d => d.Id).ToList(),

                ParentsCategories = _appDbContext.ParentsCategories
                .Include(p => p.Features)
                .Include(p => p.Models)
                .OrderByDescending(d => d.Id)
                .ToList()
            };
            return View(homeVM);
        }














        public ActionResult Details(int id)
        {
            List<Product> productList = GetAllProducts();

            Product selectedProduct = productList.FirstOrDefault(p => p.Id == id);

            if (selectedProduct != null)
            {
                ViewBag.SelectedProductImageUrls = selectedProduct.ProductİmageColor
          .Select(color => $"~/RootAllPictures/img/{color}.png")
          .ToList();
                selectedProduct.ViewCount++;
                UpdateProduct(selectedProduct);

                HomeVM model = new HomeVM
                {
                    Features=_appDbContext.Features.OrderByDescending(d => d.Id).ToList(),
                    Products = _appDbContext.Products.Include(d=>d. Features).OrderByDescending(d => d.Id).ToList(),
                    Sliders = _appDbContext.Sliders.OrderByDescending(d => d.Id).ToList(),
                    ParentsCategories = _appDbContext.ParentsCategories
                        .Include(d => d.Features)
                        .Include(d=>d.Models)
                        .OrderByDescending(d=>d.Id)
                        .ToList(),
                    SelectedProduct = selectedProduct,
                    SameProductCodeProducts = productList.Where(p => p.ProductCode == selectedProduct.ProductCode).ToList()
                };

                ViewBag.SelectedProduct = selectedProduct;
                ViewBag.SameProductCodeProducts = model.SameProductCodeProducts;

                return View("Details", model);
            }

            return RedirectToAction("Index");
        }


        public List<Product> GetAllProducts()
        {
            List<Product> productList;

           
                productList = _appDbContext.Products.ToList(); // Products tablosundaki tüm ürünleri liste olarak alır
            

            return productList;
        }

        private Product GetProductById(int id)
        {
            Product product = _appDbContext.Products
                .FirstOrDefault(p => p.Id == id);
            return product;
        }

        private void UpdateProduct(Product product)
        {
            Product? existingProduct = _appDbContext.Products.FirstOrDefault(p => p.Id == product.Id);

            if (existingProduct != null)
            {

               
                existingProduct.ViewCount = product.ViewCount;



                _appDbContext.SaveChanges();

            }
        }
        public async Task<IActionResult> MegaEndirimler()
        {
            HomeVM? homeVM = new HomeVM()
            {
                Products = _appDbContext.Products.ToList(),
                Sliders = _appDbContext.Sliders.ToList(),
                ParentsCategories = _appDbContext.ParentsCategories
                .Include(d => d.Features)
                .ToList(),

                //Services = await _appDbContext.Services.Where(c => !c.IsDeleted)
                //.Include(s=>s.Category)
                //.Include(s=>s.ServiceImages)
                //.ToListAsync()
            };
            return View(homeVM);
        }

        public async Task<IActionResult> Endirimler()
        {
            HomeVM? homeVM = new HomeVM()
            {
                Products = _appDbContext.Products.ToList(),
                Sliders = _appDbContext.Sliders.ToList(),
                ParentsCategories = _appDbContext.ParentsCategories
                .Include(d => d.Features)
                .ToList(),

                //Services = await _appDbContext.Services.Where(c => !c.IsDeleted)
                //.Include(s=>s.Category)
                //.Include(s=>s.ServiceImages)
                //.ToListAsync()
            };
            return View(homeVM);
        }

    }
}