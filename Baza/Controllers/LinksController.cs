using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Baza.Models;
using Microsoft.AspNetCore.Http;
using ReflectionIT.Mvc.Paging;

namespace Baza.Controllers
{
    public class LinksController : Controller
    {
        private readonly LinkAggregator _context;

        public LinksController(LinkAggregator context)
        {
            _context = context;
        }

        // GET: Links
        public async Task<IActionResult> Index(int page = 1)
        {
            //var linkAggregator = _context.Links.Include(l => l.Users);
            //return View(await linkAggregator.ToListAsync());
            var linkAggregator = _context.Links.Include(l => l.Users).OrderBy(l => l.LinkId);
            return View(await PagingList.CreateAsync(linkAggregator, 3, page));
        }

        // GET: Links/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var links = await _context.Links
                .Include(l => l.Users)
                .FirstOrDefaultAsync(m => m.LinkId == id);
            if (links == null)
            {
                return NotFound();
            }

            return View(links);
        }

        // GET: Links/Create
        public IActionResult Create()
        {
            //ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email");
            return View();
        }

        // POST: Links/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LinkId,Name,Link,Date,UserId")] Links links)
        {
            if (ModelState.IsValid)
            {
                links.Date = DateTime.Now;
                links.UserId = (int)HttpContext.Session.GetInt32("UserID");
                _context.Add(links);
                await _context.SaveChangesAsync();
                //ViewBag.Message = "Link dodany to agregatora!";
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Index", "Users", new { area = "" });
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", links.UserId);
            //return View(links);
            return RedirectToAction("Index", "Users", new { area = "" });
        }

        // GET: Links/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var links = await _context.Links.FindAsync(id);
            if (links == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", links.UserId);
            return View(links);
        }

        // POST: Links/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LinkId,Name,Link,Date,UserId")] Links links)
        {
            if (id != links.LinkId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(links);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LinksExists(links.LinkId))
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
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", links.UserId);
            return View(links);
        }

        // GET: Links/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var links = await _context.Links
                .Include(l => l.Users)
                .FirstOrDefaultAsync(m => m.LinkId == id);
            if (links == null)
            {
                return NotFound();
            }

            return View(links);
        }

        // POST: Links/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var links = await _context.Links.FindAsync(id);
            _context.Links.Remove(links);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LinksExists(int id)
        {
            return _context.Links.Any(e => e.LinkId == id);
        }
    }
}
