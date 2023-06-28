using System.Linq;
using Microsoft.AspNetCore.Mvc;
using FinalArizon.DAL;
using FinalArizon.Models;
using Microsoft.EntityFrameworkCore;

public class SearchController : Controller
{
    private readonly AppDbContext _dbContext;

    public SearchController(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public IActionResult Search(string q, string description, decimal? minPrice, decimal? maxPrice)
    {
        if (string.IsNullOrEmpty(q) && string.IsNullOrEmpty(description) && minPrice == null && maxPrice == null)
        {
            return View();
        }

       
     var query = _dbContext.Products
        .Include(p => p.Model)
        .Include(p => p.Features)
        .AsQueryable();

        if (!string.IsNullOrEmpty(q))
        {
            string searchQuery = q.ToLower();
            query = query.Where(p => p.Name.ToLower().Contains(searchQuery) ||
                                     p.Model.Name.ToLower().Contains(searchQuery) ||
                                     p.Features.Any(f => f.BrandName.ToLower().Contains(searchQuery)) ||
                                     p.Features.Any(f => f.ModelName.ToLower().Contains(searchQuery)));
        }

        if (!string.IsNullOrEmpty(description))
        {
            string searchDescription = description.ToLower();
            query = query.Where(p => p.Description.ToLower().Contains(searchDescription));
        }

        var searchResults = query.ToList();
        return View(searchResults);
    }
}
