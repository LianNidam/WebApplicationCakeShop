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

/// <summary>
/// to do!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// </summary>

namespace WebApplicationCakeShop.Controllers
{
    public class CartsController : Controller
    {
        private readonly WebApplicationCakeShopContext _context;

        public CartsController(WebApplicationCakeShopContext context)
        {
            _context = context;
        }

        // GET: Carts
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                List<Cart> applicationDbContext = _context.Cart.Include(c => c.User).Include(p => p.Cakes).ToList();

                foreach (Cart cart in applicationDbContext)
                {
                    cart.User = _context.User.FirstOrDefault(u => u.Id == cart.UserId);
                }
                return View(applicationDbContext);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }



        /// <summary>
        /// 
        /// searching for an item in the cart 
        /// for expample - 
        [Authorize]
        public IActionResult Search(string query)
        {
            try
            {
                String userName = User.Identity.Name;

                User user = _context.User.FirstOrDefault(x => x.Username == userName);

                Cart cart = _context.Cart.Include(db => db.Cakes).FirstOrDefault(x => x.UserId == user.Id);

                if (query == null)
                    return View("MyCart", cart);

                List<Cake> cakes = cart.Cakes.Where(p => p.Title.Contains(query) || p.Body.Contains(query)).ToList();
                cart.Cakes = cakes;

                return View("MyCart", cart);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        /// all good ^

        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchCart(string query)
        {
            try
            {

                List<Cart> carts = _context.Cart.Where(c => c.User.Username.Contains(query)).Include(p => p.Cakes).ToList();

                foreach (Cart c in carts)
                {
                    c.User = _context.User.FirstOrDefault(u => u.Id == c.UserId);
                }

                return PartialView(carts);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // GET: Carts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                var cart = await _context.Cart
                    .Include(c => c.User)
                    .Include(p => p.Cakes)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cart == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }
                cart.User = _context.User.FirstOrDefault(u => u.Id == cart.UserId);

                return View(cart);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //all good in here

        // GET: Carts/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Firstname");
            return View();
        }

        // POST: Carts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TotalPrice")] Cart cart)
        {
            if (ModelState.IsValid)
            {
                _context.Add(cart);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Set<User>(), "Id", "Firstname", cart.UserId);
            return View(cart);
        }

        // GET: Carts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                var cart = await _context.Cart.Include(p => p.Cakes).FirstOrDefaultAsync(m => m.Id == id);
                if (cart == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }
                //need to see adrees in here
                ViewData["UserId"] = new SelectList(_context.User, "Id", "Adress", cart.UserId);
                return View(cart);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        private bool CartExists(int id)
        {
            return _context.Cart.Any(e => e.Id == id);
        }
        // POST: Carts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TotalPrice")] Cart cart)
        {
            try
            {
                if (id != cart.Id)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        _context.Update(cart);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CartExists(cart.Id))
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
                //need to change adress in here
                ViewData["UserId"] = new SelectList(_context.User, "Id", "Adress", cart.UserId);
                return View(cart);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // GET: Carts/Delete/5
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                var cart = await _context.Cart
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(m => m.Id == id);
                if (cart == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }
                cart.User = _context.User.FirstOrDefault(u => u.Id == cart.UserId);
                return View(cart);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // POST: Carts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {

                var cart = await _context.Cart
                   .Include(c => c.User)
                   .Include(p => p.Cakes)
                   .FirstOrDefaultAsync(m => m.Id == id);
                cart.Cakes.Clear();
                cart.TotalPrice = 0;
                _context.Update(cart);


                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //added from out project
        public IActionResult MyCart()
        {
            try
            {
                String userName = HttpContext.User.Identity.Name;
                User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
                Cart cart = _context.Cart.FirstOrDefault(x => x.UserId == user.Id);
                cart.Cakes = _context.Cake.Where(x => x.Carts.Contains(cart)).ToList();

                if (cart == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }

                return View(cart);
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        [HttpPost, ActionName("AddToCart")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> AddToCart(int id) //product id
        {
            try
            {
                Cake cake = _context.Cake.Include(db => db.Carts).FirstOrDefault(x => x.Id == id);
                String userName = HttpContext.User.Identity.Name;
                User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
                Cart cart = _context.Cart.Include(db => db.Cakes)
                 .FirstOrDefault(x => x.UserId == user.Id);


                if (user.Cart.Cakes == null)
                    user.Cart.Cakes = new List<Cake>();
                if (cake.Carts == null)
                    cake.Carts = new List<Cart>();

                if (!(cart.Cakes.Contains(cake) && cake.Carts.Contains(cart)))
                {

                    user.Cart.Cakes.Add(cake);
                    cake.Carts.Add(cart);
                    user.Cart.TotalPrice += cake.Price;
                    _context.Update(cart);
                    _context.Update(cake);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(MyCart));
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // POST: Carts/removeProduct/5
        [HttpPost, ActionName("RemoveProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveProduct(int id)
        {
            try
            {
                Cake product = _context.Cake.FirstOrDefault(x => x.Id == id);
                String userName = HttpContext.User.Identity.Name;

                User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
                Cart cart = _context.Cart.Include(db => db.Cakes)
                    .FirstOrDefault(x => x.UserId == user.Id);

                if (product != null)
                {
                    cart.Cakes.Remove(product);
                    cart.TotalPrice -= product.Price;
                }

                _context.Attach<Cart>(cart);
                _context.Entry(cart).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MyCart));
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        [Authorize]
        public async Task<IActionResult> AfterPayment()
        {
            try
            {
                String userName = HttpContext.User.Identity.Name;
                User user = _context.User.FirstOrDefault(x => x.Username.Equals(userName));
                if (user == null)
                {
                    return RedirectToAction("PageNotFound", "Home");
                }
                Cart cart = _context.Cart.Include(db => db.Cakes).FirstOrDefault(x => x.UserId == user.Id);

                int i = cart.Cakes.RemoveAll(p => p.Id == p.Id);
                cart.TotalPrice = 0;

                _context.Attach<Cart>(cart);
                _context.Entry(cart).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return View();
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
    }
}
