using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using hatshop.Data;
using hatshop.Models;
using hatshop.ViewModels;

namespace hatshop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HatsController : Controller
    {
        private readonly HatShopDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HatsController(HatShopDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: Hats
        public async Task<IActionResult> Index()
        {
            return View(await _context.Hats.Include(h => h.Category).ToListAsync());
        }

        // GET: Hats/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.Include(h => h.Category)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (hat == null)
            {
                return NotFound();
            }

            ViewData["Picture"] = String.IsNullOrEmpty(hat.PicturePath) ? "default.png" : hat.PicturePath;
            return View(hat);
        }

        // GET: Hats/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", null);
            ViewData["Picture"] = "default.png";
            return View();
        }

        // POST: Hats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(HatPictureViewModel hatPictureViewModel)
        {
            if (ModelState.IsValid)
            {
                hatPictureViewModel.Hat.PicturePath = String.IsNullOrEmpty(hatPictureViewModel.Hat.PicturePath)
                ? UploadedFile(hatPictureViewModel, null)
                : "default.png";

                _context.Add(hatPictureViewModel.Hat);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Picture"] = "default.png";
            return View(hatPictureViewModel);
        }

        // GET: Hats/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats.FindAsync(id);

            if (hat == null)
            {
                return NotFound();
            }

            HatPictureViewModel hatPictureViewModel = new HatPictureViewModel
            {
                Hat = hat
            };

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", null);
            ViewData["Picture"] = String.IsNullOrEmpty(hat.PicturePath) ? "default.png" : hat.PicturePath;
            return View(hatPictureViewModel);
        }

        // POST: Hats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, HatPictureViewModel hatPictureViewModel)
        {
            if (id != hatPictureViewModel.Hat.Id)
            {
                return NotFound();
            }

            var path = await _context.Hats.Where(h => h.Id == id).Select(h => h.PicturePath).FirstOrDefaultAsync();

            if (ModelState.IsValid)
            {
                try
                {
                    hatPictureViewModel.Hat.PicturePath = String.IsNullOrEmpty(hatPictureViewModel.Hat.PicturePath)
                    ? UploadedFile(hatPictureViewModel, path)
                    : path;

                    _context.Update(hatPictureViewModel.Hat);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HatExists(hatPictureViewModel.Hat.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Picture"] = "default.png";
            return View(hatPictureViewModel);
        }

        // GET: Hats/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hat = await _context.Hats
                .FirstOrDefaultAsync(m => m.Id == id);
            if (hat == null)
            {
                return NotFound();
            }

            return View(hat);
        }

        // POST: Hats/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var hat = await _context.Hats.FindAsync(id);
            _context.Hats.Remove(hat);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HatExists(int id)
        {
            return _context.Hats.Any(e => e.Id == id);
        }

        private string UploadedFile(HatPictureViewModel model, string oldPath)
        {
            string uniqueFileName = null;

            if (model.Picture != null)
            {
                string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(model.Picture.FileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Picture.CopyTo(fileStream);
                }
                if (oldPath != null && oldPath != "default.png")
                    System.IO.File.Delete(Path.Combine(uploadsFolder, oldPath));
            }
            return uniqueFileName;
        }
    }
}
