using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SocialNetwork.Data
{
    public class Event
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public Post? Post { get; set; }
        public Comment? Comment { get; set; }
        public Like? Like { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
