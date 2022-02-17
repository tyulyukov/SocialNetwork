using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SocialNetwork.Data.Configurations;
using System;

namespace SocialNetwork.Data
{
    public class ApplicationDbContext : IdentityDbContext<Entities.Profile>
    {
        public DbSet<Entities.Post> Posts { get; set; }
        public DbSet<Entities.Like> Likes { get; set; }
        public DbSet<Entities.Event> Events { get; set; }
        public DbSet<Entities.Comment> Comments { get; set; }
        public DbSet<Entities.Image> Images { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new LikeConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
            builder.ApplyConfiguration(new ProfileConfiguration());
        }
    }
}
