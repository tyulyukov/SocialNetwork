using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
    public class LikeController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<Profile> _userManager;

        public LikeController(UserManager<Profile> userManager, ApplicationDbContext context)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("{id}")]
        public async Task<ActionResult<Like>> LikePost(Guid id)
        {
            var post = _context.Posts.First(p => p.Id == id);

            if (post == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            if (_context.Likes?.FirstOrDefault(l => l.Post == post && l.Author == user) != null)
                return BadRequest("This user is already liked this post");

            Like like = new Like();
            like.Id = Guid.NewGuid();
            like.CreatedAt = DateTime.Now;
            like.Author = user;
            like.Post = post;

            _context.Likes.Add(like);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // DELETE: api/Likes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLike(Guid id)
        {
            var user = await _userManager.GetUserAsync(User);

            var like = _context.Likes.First(l => l.Post.Id == id && l.Author == user);

            if (like == null)
                return BadRequest("This user didnt liked this post");

            _context.Likes.Remove(like);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
