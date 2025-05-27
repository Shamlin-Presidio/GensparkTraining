using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Chirper.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;


        public ICollection<Tweet> Tweets { get; set; } = new List<Tweet>();
        [InverseProperty("Following")] // users that follow this user
        public ICollection<User> Followers { get; set; } = new List<User>();

        [InverseProperty("Followers")] // Users this user is following
        public ICollection<User> Following { get; set; } = new List<User>();
    }
}