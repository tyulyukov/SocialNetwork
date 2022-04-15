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

            var posts = await _context.Posts
                .Where(p => p.Author == profile)
                .Include(p => p.Attachments)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.Posts = posts;
            ViewBag.LikedPosts = posts.Where(p => p.Likes.Any(l => l.Author == user)).ToList();

            ViewBag.IsFollowing = user.Following?.Any(f => f.Id == profile.Id) == true;
           
            return View(profile);
        }

        [Route("Profiles/{username?}/Photos")]
        public async Task<IActionResult> Photos(string username)
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
                return RedirectToAction("Photos", "MyProfile");

            var photos = await _context.Images
                .Where(p => p.Post.Author == profile)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.Photos = photos;
            ViewBag.IsFollowing = user.Following?.Any(f => f.Id == profile.Id) == true;

            return View(profile);
        }

        [Route("Profiles/{username?}/Followers")]
        public async Task<IActionResult> Followers(string username)
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
                return RedirectToAction("Followers", "MyProfile");

            var followers = _context.Users
                .Where(u => u.Following.Contains(profile))
                .Include(u => u.Following)
                .Include(u => u.Followers)
                .ToList();

            ViewBag.Followers = followers;
            ViewBag.IsFollowing = user.Following?.Any(f => f.Id == profile.Id) == true;

            return View(profile);
        }

        [Route("Profiles/{username?}/Following")]
        public async Task<IActionResult> Following(string username)
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
                return RedirectToAction("Following", "MyProfile");

            var following = _context.Users
                .Where(u => u.Followers.Contains(profile))
                .Include(u => u.Following)
                .Include(u => u.Followers)
                .ToList();

            ViewBag.Following = following;
            ViewBag.IsFollowing = user.Following?.Any(f => f.Id == profile.Id) == true;

            return View(profile);
        }
    }
}
