using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hatshop.Data;
using hatshop.Models;

namespace hatshop.Controllers
{
    public class ShopController : Controller
    {
        private readonly HatShopDbContext _context;

        public ShopController(HatShopDbContext context)
        {
            _context = context;
        }

        // GET: Hats
        public IActionResult Index(int? page, string filter, int? categoryId)
        {
            var allHats = _context.Hats.Include(h => h.Category).AsQueryable();
            allHats = !String.IsNullOrEmpty(filter) ? allHats.Where(h => h.Name.Contains(filter)) : allHats;
            allHats = categoryId != null ? allHats.Where(h => h.CategoryId == categoryId) : allHats;
            var hatsPage = allHats.OrderBy(h => h.Name).Skip((page - 1 ?? 0) * 6).Take(6);
            
            ViewData["NumberOfHats"] = allHats.Count();
            ViewData["Categories"] = _context.Categories;
            return View(hatsPage);
        }
    }
}
