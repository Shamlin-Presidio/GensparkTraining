using System.Collections.Generic;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Chirper.Models
{
    public class Tweet
    {
        [Key]
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; } = null!;

        public ICollection<Like> Likes { get; set; } = new List<Like>();
        public ICollection<TweetHashtag> TweetHashtags { get; set; } = new List<TweetHashtag>();
    }

}