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
    public class CategoriesController : Controller
    {
        private readonly WebApplicationCakeShopContext _context;

        public CategoriesController(WebApplicationCakeShopContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            return View(await _context.Category.ToListAsync());
        }

        // GET: Categories/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Categories/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("Id,Name")] Category category)
        {
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Categories/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Category.FindAsync(id);
            _context.Category.Remove(category);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.Id == id);
        }

        /// <summary>
        /// group by
        /// </summary>
        
        [HttpPost]
        public IActionResult GroupByPrice()
        {
            try
            {
                var groups = from p in _context.Cake.ToList()
                             group p by p.Price
                into g
                             orderby g.Key
                             select g;

                List<Cake> cakes = new List<Cake>();
                foreach (var cake in groups)
                {
                    for (int i = 0; i < cake.ToList().Count; i++)
                    {
                        cakes.Add(cake.ElementAt(i));
                    }
                }

                return View("AllCakes", cakes);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //need to check this!!^^

        /// <summary>
        /// Join
        /// </summary>
        public async Task<IActionResult> AllCakes()
        {
            try
            {
                var cakes =
                    from category in _context.Category
                    join cake in _context.Cake on category.Id equals cake.CategoryId
                    orderby category.Id
                    select cake;

                return View(await cakes.ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        public async Task<IActionResult> Milky()
        {
            return View(await _context.Cake.Where(x => x.CategoryId == 9).ToListAsync());
        }
        public async Task<IActionResult> NoEggs()
        {
            return View(await _context.Cake.Where(x => x.CategoryId == 10).ToListAsync());
        }
        public async Task<IActionResult> Parve()
        {
            return View(await _context.Cake.Where(x => x.CategoryId == 7).ToListAsync());
        }
        public async Task<IActionResult> Special()
        {
            return View(await _context.Cake.Where(x => x.CategoryId == 8).ToListAsync());
        }
        

        public async Task<IActionResult> Vegan()
        {
            return View(await _context.Cake.Where(x => x.CategoryId == 5).ToListAsync());
        }

        public async Task<IActionResult> WithoutBaking()
        {
            return View(await _context.Cake.Where(x => x.CategoryId == 6).ToListAsync());
        }



        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Search(string query)
        {
            try
            {
                return Json(await _context.Category.Where(c => c.Name.Contains(query)).ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }





    }
}
