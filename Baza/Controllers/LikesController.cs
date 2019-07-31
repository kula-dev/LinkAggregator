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
    public class LikesController : Controller
    {
        private readonly LinkAggregator _context;

        public LikesController(LinkAggregator context)
        {
            _context = context;
        }

        // POST: Likes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Like([Bind("LikeId,UserID,LinkID")] Likes likes, int id)
        {
            likes.LinkID = id;
            likes.UserID = (int)HttpContext.Session.GetInt32("UserID");
            if (ModelState.IsValid)
            {
                _context.Add(likes);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            ViewData["LinkID"] = new SelectList(_context.Links, "LinkId", "Link", likes.LinkID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", likes.UserID);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // GET: Likes
        public async Task<IActionResult> Index()
        {
            var linkAggregator = _context.Likes.Include(l => l.Links).Include(l => l.Users);
            return View(await linkAggregator.ToListAsync());
        }

        // GET: Likes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var likes = await _context.Likes
                .Include(l => l.Links)
                .Include(l => l.Users)
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (likes == null)
            {
                return NotFound();
            }

            return View(likes);
        }

        // GET: Likes/Create
        public IActionResult Create()
        {
            ViewData["LinkID"] = new SelectList(_context.Links, "LinkId", "Link");
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Likes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LikeId,UserID,LinkID")] Likes likes)
        {
            if (ModelState.IsValid)
            {
                _context.Add(likes);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["LinkID"] = new SelectList(_context.Links, "LinkId", "Link", likes.LinkID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", likes.UserID);
            return View(likes);
        }

        // GET: Likes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var likes = await _context.Likes.FindAsync(id);
            if (likes == null)
            {
                return NotFound();
            }
            ViewData["LinkID"] = new SelectList(_context.Links, "LinkId", "Link", likes.LinkID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", likes.UserID);
            return View(likes);
        }

        // POST: Likes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LikeId,UserID,LinkID")] Likes likes)
        {
            if (id != likes.LikeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(likes);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LikesExists(likes.LikeId))
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
            ViewData["LinkID"] = new SelectList(_context.Links, "LinkId", "Link", likes.LinkID);
            ViewData["UserID"] = new SelectList(_context.Users, "UserId", "Email", likes.UserID);
            return View(likes);
        }

        // GET: Likes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var likes = await _context.Likes
                .Include(l => l.Links)
                .Include(l => l.Users)
                .FirstOrDefaultAsync(m => m.LikeId == id);
            if (likes == null)
            {
                return NotFound();
            }

            return View(likes);
        }

        // POST: Likes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var likes = await _context.Likes.FindAsync(id);
            _context.Likes.Remove(likes);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LikesExists(int id)
        {
            return _context.Likes.Any(e => e.LikeId == id);
        }
    }
}
