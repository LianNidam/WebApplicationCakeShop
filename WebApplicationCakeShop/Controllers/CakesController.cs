using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public async Task<IActionResult> SearchTable(string query)
        {
            try
            {
                var applicationDbContext = _context.Cake.Include(p => p.Category);
                return PartialView(await applicationDbContext.Where(p => p.Category.Name.Contains(query)).ToListAsync());
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
        [Authorize(Roles = "Admin")]
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



        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchPtable(string query)
        {
            try
            {
                var applicationDbContext = _context.Cake.Include(p => p.Category);
                return PartialView(await applicationDbContext.Where(p => p.Title.Contains(query)).ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }



      


        public async Task<IActionResult> Search1(string searchString)
        {
            var cakes = from m in _context.Cake
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                cakes = cakes.Where(s => s.Title.Contains(searchString));
            }

            return View("searchlist", await cakes.ToListAsync());
        }

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


        //public IActionResult searchlist()
        //{
        //    return View();
        //}


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


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public ActionResult Statistics()
        {
            try
            {
                //statistic-1- what is the most "popular" cakes\
                ICollection<Stat> statistic1 = new Collection<Stat>();
                var result = from p in _context.Cake.Include(o => o.Carts)
                             where (p.Carts.Count) > 0
                             orderby (p.Carts.Count) descending
                             select p;
                foreach (var v in result)
                {
                    statistic1.Add(new Stat(v.Title, v.Carts.Count()));
                }

                ViewBag.data1 = statistic1;

                //finish first statistic
                //statistic 2-what is the most common age of the users
                ICollection<Stat> statistic2 = new Collection<Stat>();
                List<User> users = _context.User.ToList();
                int currentYear = DateTime.Today.Year;
                Dictionary<int, int> result2 = new Dictionary<int, int>();
                //foreach (User item in users)
                //{
                //    if (!result2.ContainsKey(currentYear - item.Age.Year))
                //    {
                //        result2.Add(currentYear - item.Age.Year, 1);
                //    }
                //    else
                //    {
                //        int count = result2.GetValueOrDefault(currentYear - item.Age.Year) + 1;
                //        result2.Remove(currentYear - item.Age.Year);
                //        result2.Add(currentYear - item.Age.Year, count);
                //    }

                //}

                foreach (var v in result2.OrderBy(k => k.Key))
                {
                    if (v.Value > 0)
                    {
                        statistic2.Add(new Stat(v.Key.ToString(), v.Value));
                    }
                }


                ViewBag.data2 = statistic2;



                //statistic-3- what category hava the biggest number of games
                ICollection<Stat> statistic3 = new Collection<Stat>();
                List<Cake> cakes = _context.Cake.ToList();
                List<Category> categories = _context.Category.ToList();
                var result3 = from prod in cakes
                              join cat in categories on prod.CategoryId equals cat.Id
                              group cat by cat.Id into G
                              select new { id = G.Key, num = G.Count() };

                var porqua = from popo in result3
                             join cat in categories on popo.id equals cat.Id
                             select new { category = cat.Name, count = popo.num };
                foreach (var v in porqua)
                {
                    if (v.count > 0)
                        statistic3.Add(new Stat(v.category, v.count));
                }

                ViewBag.data3 = statistic3;
                return View();
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }




    }


}

public class Stat
{
    public string Key;
    public int Values;
    public Stat(string key, int values)
    {
        Key = key;
        Values = values;
    }
}