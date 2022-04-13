using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Data.Entities
{
    public class Comment
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public String Text { get; set; }
        public Post Post { get; set; }
        public Profile Author { get; set; }
        public Comment? RepliedComment { get; set; }
        public List<Comment> ChildrenComments { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}