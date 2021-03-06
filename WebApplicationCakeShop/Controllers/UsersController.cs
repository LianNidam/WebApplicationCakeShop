using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplicationCakeShop.Data;
using WebApplicationCakeShop.Models;

namespace WebApplicationCakeShop.Controllers
{
    public class UsersController : Controller
    {
        private readonly WebApplicationCakeShopContext _context;

        public UsersController(WebApplicationCakeShopContext context)
        {
            _context = context;
        }

        // GET: Users
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            try
            {
                return View(await _context.User.ToListAsync());
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }

        // GET: Users/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id == 0)
            {
                return AccessDenied();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

       
        /// login, logout, accses dennied
        /// 

        //LogOut Option
        public async Task<IActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login");
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }




       

        //login post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("Id,Username,Password")] User user)
        {

            try
            {
                var q = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);
                if (q != null)
                {
                    Signin(q);
                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {
                    ViewData["Error"] = "Username and/or password are incorrect.";
                    return View(user);
                }
            }
            catch { return RedirectToAction("PageNotFound", "Home"); }
        }
        //singin func post

        private async void Signin(User account)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, account.Username),
                    new Claim(ClaimTypes.Role, account.Type.ToString()),
                };

            var claimsIdentity = new ClaimsIdentity(
                claims, CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }


       
        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,Username,Password,Firstname,Lastname,Address,Phone")] User user)
        {
            //if (ModelState.IsValid)
            //{
            //    _context.Add(user);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //return View(user);

            if (ModelState.IsValid)
            {
                var q = _context.User.FirstOrDefault(u => u.Username == user.Username);
                var w = _context.User.FirstOrDefault(w => w.Address == user.Address);

                if (q == null && w == null)
                {
                    _context.Add(user);
                    await _context.SaveChangesAsync();

                    var u = _context.User.FirstOrDefault(u => u.Username == user.Username && u.Password == user.Password);

                    Signin(u);

                    return RedirectToAction(nameof(Index), "Home");
                }
                else
                {

                    //need to fix the error massage
                    ViewBag.ErrorMessage = "Can't do it";
                    ViewData["ErrorMessage"] = "Can't do it";
                }
            }
            return View(user);
        }

        // GET: Users/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (id == 0)
            {
                return AccessDenied();
            }

            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return View(user);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Username,Password,Type,Firstname,Lastname,Address,Phone")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(user.Id))
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
            return View(user);
        }

        // GET: Users/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.User
                .FirstOrDefaultAsync(m => m.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.User.FindAsync(id);
            _context.User.Remove(user);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "Admin")]
        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }

        //LogIn Option
        public IActionResult Login()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Search()
        {
            return View();
        }
        //You Don't have premotion..
        public IActionResult AccessDenied()
        {
            return View();
        }
        // GET: Users/Register
        public IActionResult Create()
        {
            return View();
        }
    }
}
