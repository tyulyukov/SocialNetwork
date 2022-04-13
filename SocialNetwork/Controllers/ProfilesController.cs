using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Data.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class ProfilesController : Controller
    {
        private readonly UserManager<Profile> _userManager;
        private readonly ApplicationDbContext _context;

        public ProfilesController(UserManager<Profile> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [Route("Profiles/{username?}")]
        public async Task<IActionResult> Index(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return NotFound();

            var profile = await _context.Users
                .Where(m => m.UserName.ToLower() == username.ToLower())
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .FirstAsync();

            if (profile == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            if (profile == user)
                return RedirectToAction("Index", "MyProfile");

            ViewBag.Posts = await _context.Posts
                .Where(p => p.Author == profile)
                .Include(p => p.Attachments)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.LikedPosts = await _context.Posts.Where(p => p.Author == profile && p.Likes.Any(l => l.Author == user)).ToListAsync();

            ViewBag.IsFollowing = user.Following?.Any(f => f.Id == profile.Id) == true;
           
            return View(profile);
        }
    }
}
