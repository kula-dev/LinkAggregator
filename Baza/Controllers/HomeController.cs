using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Baza.Models;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;

namespace Baza.Controllers
{
    public class HomeController : Controller
    {
        private readonly LinkAggregator _context;

        public HomeController(LinkAggregator context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            //var linkAggregator = _context.Links.Include(l => l.Users);
            //var linkAggregator = _context.Links.Include(l => l.Users).Include(l => l.Likes);
            //return View(await linkAggregator.ToListAsync());
            //item.Likes.Where(l => l.LinkID == item.LinkId).Count();
            var linkAggregator = _context.Links.Include(l => l.Users).Include(l => l.Likes)
                .OrderByDescending(o => o.Likes.Where(l => l.LinkID == o.LinkId).Count());
            return View(await PagingList.CreateAsync(linkAggregator, 3, page));
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> View22()
        {
            ViewData["Message"] = "hłe hłe hłe hłe";
            var linkAggregator = _context.Links.Include(l => l.Users);
            return View(await linkAggregator.ToListAsync());
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
