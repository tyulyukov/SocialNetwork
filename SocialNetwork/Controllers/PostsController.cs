using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Data.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Controllers
{
    public class PostsController : Controller
    {
        private readonly UserManager<Profile> _userManager;
        private readonly ApplicationDbContext _context;

        public PostsController(UserManager<Profile> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        [Route("Posts/{id?}")]
        public async Task<IActionResult> Index(Guid id)
        {
            var post = _context.Posts
                .Where(p => p.Id == id)
                .Include(p => p.Likes)
                .Include(p => p.Attachments)
                .Include(p => p.Author)
                .First();

            if (post == null)
                return NotFound();

            var user = await _userManager.GetUserAsync(User);

            ViewBag.Comments = _context.Comments
                .Where(c => c.Post.Id == id && c.RepliedComment == null)
                .Include(c => c.Author)
                .Include(c => c.RepliedComment)
                .Include(c => c.ChildrenComments.OrderBy(cc => cc.CreatedAt))
                .ToList();

            ViewBag.IsLiked = post.Likes.Any(l => l.Author == user);
            ViewBag.IsMyPost = post.Author == user;

            return View(post);
        }

        [Route("Posts/SendComment/")]
        public async Task<IActionResult> SendComment([Bind("Id, Text")] Comment comment, Guid postId)
        {
            var post = _context.Posts.Find(postId);

            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(comment.Text))
            {
                var user = await _userManager.GetUserAsync(User);

                if (post == null)
                    return NotFound();

                comment.Id = Guid.NewGuid();
                comment.Author = user;
                comment.CreatedAt = DateTime.Now;
                comment.RepliedComment = null;
                comment.ChildrenComments = new();
                comment.Post = post;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { id = post.Id });
        }

        [Route("Posts/SendCommentReply/")]
        public async Task<IActionResult> SendCommentReply([Bind("Id, Text")] Comment comment, Guid postId, Guid repliedCommentId)
        {
            var post = _context.Posts.Find(postId);

            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(comment.Text))
            {
                var user = await _userManager.GetUserAsync(User);

                var repliedComment = _context.Comments.Find(repliedCommentId);

                if (post == null)
                    return NotFound();

                comment.Id = Guid.NewGuid();
                comment.Author = user;
                comment.CreatedAt = DateTime.Now;
                comment.RepliedComment = repliedComment;
                comment.ChildrenComments = new();
                comment.Post = post;

                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index), new { id = post.Id });
        }
    }
}
