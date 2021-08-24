using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationCakeShop.Data;
using WebApplicationCakeShop.Models;

namespace WebApplicationCakeShop.Controllers
{
    public class CakesController : Controller
    {
        private readonly WebApplicationCakeShopContext _context;

        public CakesController(WebApplicationCakeShopContext context)
        {
            _context = context;
        }
        //working


        // GET: Cakes
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            //var webApplicationCakeShopContext = _context.Cake.Include(c => c.Category);
            //return View(await webApplicationCakeShopContext.ToListAsync());
            try
            {
                var applicationDbContext = _context.Cake.Include(p => p.Category);
                return View(await applicationDbContext.ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }

        }
        //------upWorking

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchPtable(string query)
        {
            try
            {
                var applicationDbContext = _context.Cake.Include(p => p.Category);
                return PartialView(await applicationDbContext.Where(p => p.Category.Name.Contains(query)).ToListAsync());//check the Name
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //------upWorking

        public async Task<IActionResult> Search(string cakeName, string price, string category)
        {
            try
            {
                int p = Int32.Parse(price);
                var applicationDbContext = _context.Cake.Include(a => a.Category).Where(a => a.Category.Name.Contains(cakeName) && a.Category.Name.Equals(category) && a.Price <= p);
                return View("searchlist", await applicationDbContext.ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        public async Task<IActionResult> Search(string cakeName)
        {
            try
            {
                var LNidam = _context.Cake.Include(a => a.Category).Where(a => a.Category.Name.Contains(cakeName));
                return View("searchlist", await LNidam.ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //------upWorking

        // GET: Cakes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                var cake = await _context.Cake
                    .Include(p => p.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cake == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                return View(cake);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //------upWorking

        // GET: Cakes/Create
        //[Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name");
            return View();
        }
        //------upWorking


        // POST: Cakes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Body,CategoryId,PhotosUrl1,PhotosUrl2,PhotosUrl3,PhotosUrl4,Price")] Cake cake)
        {
            try
            {

                if (ModelState.IsValid)
                {
                    _context.Add(cake);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", cake.CategoryId);
                return View(cake);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }

        }
        //------upWorking



        // GET: Cakes/Edit/5
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");

                }

                var cake = await _context.Cake.FindAsync(id);
                if (cake == null)
                {
                    return RedirectToAction("PageNotFound", "Home");

                }
                ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", cake.CategoryId);
                return View(cake);

            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //------upWorking



        // POST: Cakes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Body,CategoryId,PhotosUrl1,PhotosUrl2,PhotosUrl3,PhotosUrl4,Price")] Cake cake)
        {
            try
            {
                if (id != cake.Id)
                {
                    return RedirectToAction("PageNotFound", "Home");

                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(cake);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CakeExists(cake.Id))
                        {
                            return RedirectToAction("PageNotFound", "Home");

                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));

                }

                ViewData["CategoryId"] = new SelectList(_context.Set<Category>(), "Id", "Name", cake.CategoryId);
                return View(cake);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //------upWorking



        // GET: Cakes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");

                }

                var cake = await _context.Cake
                .Include(c => c.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
                if (cake == null)
                {
                    return RedirectToAction("PageNotFound", "Home");

                }
                return View(cake);
            }

            catch { return RedirectToAction("PageNotFound", "Home"); }

        }
        //------upWorking



        // POST: Cakes/Delete/5
        [HttpPost, ActionName("Delete")]
        //[Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var cake = await _context.Cake.FindAsync(id);
                _context.Cake.Remove(cake);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

            private bool CakeExists(int id)
        {
            return _context.Cake.Any(e => e.Id == id);
        }

    }

}
