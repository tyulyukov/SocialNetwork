using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace SocialNetwork.Data.Entities
{
    public class Profile : IdentityUser
    {
        public String FullName { get; set; }
        public String? Description { get; set; }
        public String AvatarImageUrl { get; set; }
        public String TelegramId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Profile> Friends { get; set; }
        public List<Profile> Followers { get; set; }
        public List<Profile> Following { get; set; }
        public List<Post> Posts { get; set; }
        public List<Like> Likes { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
