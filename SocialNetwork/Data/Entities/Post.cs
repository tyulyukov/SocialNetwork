using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Data.Entities
{
    public class Post
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public String Text { get; set; }
        public Profile Author { get; set; }
        public List<Image> Attachments { get; set; }
        public List<Comment> Comments { get; set; }
        public List<Like> Likes { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
