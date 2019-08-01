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

        public IActionResult Create()
        {
            return View();
        }

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
                ViewBag.MessageLinkAdd = "Link dodany to agregatora!";
                return View();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", links.UserId);
            return View();
        }

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
                return RedirectToAction("Index", "Users", new { area = "" });
            }
            ViewData["UserId"] = new SelectList(_context.Users, "UserId", "Email", links.UserId);
            return View(links);
        }

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

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var links = await _context.Links.FindAsync(id);
            _context.Links.Remove(links);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Users", new { area = "" });
        }

        private bool LinksExists(int id)
        {
            return _context.Links.Any(e => e.LinkId == id);
        }
    }
}
