using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Data.Entities;
using System.Linq;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class SearchController : Controller
    {
        private readonly UserManager<Profile> _userManager;
        private readonly ApplicationDbContext _context;

        public SearchController(UserManager<Profile> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("Search/{username?}")]
        public IActionResult Index(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return NotFound();

            var users = _context.Users
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .Where(u => u.UserName.ToLower().Contains(username.ToLower()) || u.FullName.ToLower().Contains(username.ToLower()))
                .ToList();

            return View(users);
        }
    }
}
