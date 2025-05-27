using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirper.Models
{
    public class TweetHashtag
    {
        [Key]
        public int TweetId { get; set; }

        [ForeignKey("TweetId")]
        public Tweet Tweet { get; set; } = null!;


        public int HashtagId { get; set; }
        [ForeignKey("HashtagId")]
        public Hashtag Hashtag { get; set; } = null!;
    }
}