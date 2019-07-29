using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Baza.Models;
using Microsoft.AspNetCore.Http;

namespace Baza.Controllers
{
    public class UsersController : Controller
    {
        private readonly LinkAggregator _context;

        public UsersController(LinkAggregator context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            //return View(await _context.Users.ToListAsync());
            var linkAggregator = _context.Links.Include(l => l.Users);
            return View(await linkAggregator.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users
                .FirstOrDefaultAsync(m => m.UserId == id);
            if (users == null)
            {
                return NotFound();
            }

            return View(users);
        }

        // GET: Users/Create
        

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var users = await _context.Users.FindAsync(id);
            if (users == null)
            {
                return NotFound();
            }
            return View(users);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("UserId,Email,Password,ConfirmPassword")] Users users)
        {
            if (id != users.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(users);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersExists(users.UserId))
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
            return View(users);
        }

        // GET: Users/Delete/5

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users users)
        {
            if (ModelState.IsValid)
            {
                _context.Add(users);
                await _context.SaveChangesAsync();
                ViewBag.Message = users.Email + " Dodany to agregatora!";
                //return RedirectToAction(nameof(Index));
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Users users)
        {
            var usr = _context.Users.Where(u => u.Email == users.Email && u.Password == users.Password).FirstOrDefault();
            if (usr != null)
            {
                HttpContext.Session.SetInt32("UserID", usr.UserId);
                HttpContext.Session.SetString("UserEmail", usr.Email.ToString());
                HttpContext.Session.SetInt32("Login", 1);
                TempData["UserID"] = usr.UserId.ToString();
                ViewData["UserEmail"] = usr.UserId.ToString();
                ViewData["Login"] = true;
                return RedirectToAction(nameof(Index));
            }
            else
                ModelState.AddModelError("", "Podane hasło lub Email są nie prawidłowe");
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Login");
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("UserEmail");
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Panel()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Panel([Bind("LinkId,Name,Link,Date,UserId")] Links links)
        {
            if (ModelState.IsValid)
            {
                _context.Add(links);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", links.UserId);
            return View(links);
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
