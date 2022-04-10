using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Data.Entities;

namespace SocialNetwork.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FollowController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public FollowController(UserManager<Profile> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult> FollowProfile(Guid id)
        {
            var profile = _context.Users
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .First(u => u.Id == id.ToString());

            if (profile == null)
                return NotFound();

            var userId = (await _userManager.GetUserAsync(User)).Id;

            var user = await _context.Users
               .Include(u => u.Followers)
               .Include(u => u.Following)
               .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == profile)
                return BadRequest("Can`t follow yourself");

            if (user.Following?.Any(f => f.Id == id.ToString()) == true)
                return BadRequest("User is already followed to this user");

            user.Following.Add(profile);
            profile.Followers.Add(user);
            _context.SaveChanges();

            return Ok();
        }

        // DELETE: api/Likes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> UnfollowProfile(Guid id)
        {
            var profile = _context.Users
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .First(u => u.Id == id.ToString());

            if (profile == null)
                return NotFound();

            var userId = (await _userManager.GetUserAsync(User)).Id;

            var user = await _context.Users
               .Include(u => u.Followers)
               .Include(u => u.Following)
               .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == profile)
                return BadRequest("Can`t unfollow yourself");

            if (user.Following?.Any(f => f.Id == id.ToString()) == false)
                return BadRequest("User is already followed to this user");

            user.Following.Remove(profile);
            profile.Followers.Remove(user);
            _context.SaveChanges();

            return Ok();
        }
    }
}
