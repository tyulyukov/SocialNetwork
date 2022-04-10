﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data;
using SocialNetwork.Data.Entities;
using SocialNetwork.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialNetwork.Controllers
{
    [Authorize]
    public class MyProfileController : Controller
    {
        private readonly UserManager<Profile> _userManager;
        private readonly SignInManager<Profile> _signInManager;
        private readonly ApplicationDbContext _context;

        public MyProfileController(UserManager<Profile> userManager, SignInManager<Profile> signInManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            if (!_signInManager.IsSignedIn(User))
                return RedirectToPage("/Account/AccessDenied", new { Area = "Identity" });

            var user = await _userManager.GetUserAsync(User);

            ViewBag.Posts = await _context.Posts
                .Where(p => p.Author == user)
                .Include(p => p.Attachments)
                .Include(p => p.Likes)
                .Include(p => p.Comments)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();

            ViewBag.LikedPosts = await _context.Posts.Where(p => p.Author == user && p.Likes.Any(l => l.Author == user)).ToListAsync();

            return View(user);
        }

        public async Task<IActionResult> SharePost([Bind("Id,Text")] Post post, IFormFile file)
        {
            if (ModelState.IsValid && !String.IsNullOrWhiteSpace(post.Text))
            {
                var user = await _userManager.GetUserAsync(User);

                post.Id = Guid.NewGuid();
                post.Author = user;
                post.CreatedAt = DateTime.Now;
                post.Attachments = new();
                post.Comments = new();
                post.Likes = new();

                if (file != null)
                {
                    var imageUrl = Media.UploadImage(file, "images");

                    if (!String.IsNullOrEmpty(imageUrl))
                        post.Attachments.Add(new Image() { Id = Guid.NewGuid(), CreatedAt = DateTime.Now, Url = imageUrl, Post = post} );
                }

                _context.Add(post);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeletePost(Guid id)
        {
            var post = await _context.Posts.FindAsync(id);

            var likes = await _context.Likes.Where(l => l.Post == post).ToListAsync();
            var comments = await _context.Comments.Where(l => l.Post == post).ToListAsync();
            var images = await _context.Images.Where(l => l.Post == post).ToListAsync();

            foreach (var image in images)
            {
                var path = Media.WebRootPath + image.Url;
                System.IO.File.Delete(path);
            }

            _context.Likes.RemoveRange(likes);
            _context.Comments.RemoveRange(comments);
            _context.Images.RemoveRange(images);
            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
