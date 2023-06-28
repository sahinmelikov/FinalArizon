using FinalArizon.DAL;
using FinalArizon.Models;
using FinalArizon.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace FinalArizon.Controllers
{
    public class BasketController : Controller
    {
        private const string COOKIES_BASKET = "basketVM";
        private readonly AppDbContext _appDbContext;

        public BasketController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private void SetBasketItemCountInViewBag()
        {
            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET] ?? "[]");
            int itemCount = basketVMs.Sum(b => b.Count);
            ViewBag.BasketItemCount = itemCount;
        }

        public IActionResult Index()
        {
            List<BasketItemVM> basketItemVMs = new List<BasketItemVM>();
            List<BasketVM> basketVMs = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET] ?? "[]");
            foreach (BasketVM item in basketVMs)
            {
                BasketItemVM basketItemVM = _appDbContext.Products
                    .Where(s => s.Id == item.ProductId)
                    .Select(s => new BasketItemVM
                    {
                        Name = s.Name,
                        Id = s.Id,
                        Description = s.Description,
                        Price = s.Price,
                        DiscountPrice = s.DiscountPrice,
                        ProductCount = item.Count,
                        ImagePath = s.ImagePath
                    })
                    .FirstOrDefault();
                if (basketItemVM != null)
                    basketItemVMs.Add(basketItemVM);
            }

            SetBasketItemCountInViewBag();

            return View(basketItemVMs);
        }

        public IActionResult AddBasket(int id)
        {
            List<BasketVM> basketVMList = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET] ?? "[]");

            BasketVM cookiesBasket = basketVMList.FirstOrDefault(s => s.ProductId == id);
            if (cookiesBasket != null)
            {
                cookiesBasket.Count++;
            }
            else
            {
                BasketVM basketVM = new BasketVM() { ProductId = id, Count = 1 };
                basketVMList.Add(basketVM);
            }

            Response.Cookies.Append(COOKIES_BASKET, JsonConvert.SerializeObject(basketVMList.OrderBy(s => s.ProductId)));

            SetBasketItemCountInViewBag();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveFromBasket(int id)
        {
            List<BasketVM> basketVMList = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET] ?? "[]");

            BasketVM cookiesBasket = basketVMList.FirstOrDefault(s => s.ProductId == id);
            if (cookiesBasket != null)
            {
                if (cookiesBasket.Count > 1)
                {
                    cookiesBasket.Count--;
                }
                else
                {
                    basketVMList.Remove(cookiesBasket);
                }
            }

            Response.Cookies.Append(COOKIES_BASKET, JsonConvert.SerializeObject(basketVMList.OrderBy(s => s.ProductId)));

            SetBasketItemCountInViewBag();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveItemFromBasket(int id)
        {
            List<BasketVM> basketVMList = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET] ?? "[]");

            BasketVM productToRemove = basketVMList.FirstOrDefault(s => s.ProductId == id);
            if (productToRemove != null)
            {
                basketVMList.Remove(productToRemove);

                Response.Cookies.Append(COOKIES_BASKET, JsonConvert.SerializeObject(basketVMList.OrderBy(s => s.ProductId)));
            }

            SetBasketItemCountInViewBag();

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateBasketItem(int productId, string action)
        {
            List<BasketVM> basketVMList = JsonConvert.DeserializeObject<List<BasketVM>>(Request.Cookies[COOKIES_BASKET] ?? "[]");

            BasketVM basketItem = basketVMList.FirstOrDefault(s => s.ProductId == productId);
            if (basketItem != null)
            {
                if (action == "increment")
                {
                    basketItem.Count++;
                }
                else if (action == "decrement")
                {
                    if (basketItem.Count > 1)
                    {
                        basketItem.Count--;
                    }
                    else
                    {
                        basketVMList.Remove(basketItem);
                    }
                }
            }

            Response.Cookies.Append(COOKIES_BASKET, JsonConvert.SerializeObject(basketVMList.OrderBy(s => s.ProductId)));

            SetBasketItemCountInViewBag();

            return RedirectToAction("Index", "Basket");
        }
        public IActionResult RemoveAllItemsFromBasket()
        {
            // Sepet öğelerini temizle
            Response.Cookies.Delete(COOKIES_BASKET);

            return RedirectToAction("Index");
        }
    }
}
