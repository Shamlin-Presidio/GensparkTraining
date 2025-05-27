using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirper.Models
{
    public class Hashtag
    {
        [Key]
        public int Id { get; set; }
        public string Tag { get; set; } = null!;

        public ICollection<TweetHashtag> TweetHashtags { get; set; } = new List<TweetHashtag>();
    }
}
