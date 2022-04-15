using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SocialNetwork.Data;
using SocialNetwork.Data.Entities;
using SocialNetwork.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly UserManager<Profile> _userManager;
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context, UserManager<Profile> userManager, ILogger<HomeController> logger)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var userId = (await _userManager.GetUserAsync(User)).Id;

            var user = await _context.Users
                .Where(u => u.Id == userId)
                .Include(u => u.Following)
                .FirstAsync();

            var posts = _context.Posts
                .Where(p => user.Following.Contains(p.Author))
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .Include(p => p.Attachments)
                .OrderByDescending(p => p.CreatedAt);

            ViewBag.LikedPosts = posts.Where(p => p.Likes.Any(p => p.Author == user)).ToList();

            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
