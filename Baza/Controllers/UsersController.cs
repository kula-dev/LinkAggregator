using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Baza.Models;
using Microsoft.AspNetCore.Http;
using System.Data.SqlClient;

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
            if (HttpContext.Session.GetInt32("Login") != 1)
                return RedirectToAction(nameof(Login));
            else
            {
                var linkAggregator = _context.Links.Include(l => l.Users);
                return View(await linkAggregator.ToListAsync());
            }
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
            if (HttpContext.Session.GetInt32("Login") != 1)
                return View();
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users users)
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
            {
                if (ModelState.IsValid)
                {
                    _context.Add(users);
                    try
                    {
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        var sqlException = ex.InnerException as System.Data.SqlClient.SqlException;

                        if (sqlException.Number == 2601 || sqlException.Number == 2627)
                        {
                            ViewBag.Message = "Już istnieje taki użytkownik!";
                            return View();
                        }
                        else
                        {
                            ViewBag.Message = "Bład podczas dodawania do bazy!";
                            return View();
                        }
                    }
                    ViewBag.Message = users.Email + " Dodany to agregatora!";
                }
                return View();
            }
            else
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
                return View();
            else
                return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(Users users)
        {
            if (HttpContext.Session.GetInt32("Login") != 1)
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
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
                else
                    ModelState.AddModelError("", "Podane hasło lub Email są nie prawidłowe");
                return View();
            }
            else
                return RedirectToAction(nameof(Index));
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Login");
            HttpContext.Session.Remove("UserID");
            HttpContext.Session.Remove("UserEmail");
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        private bool UsersExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
