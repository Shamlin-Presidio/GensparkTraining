using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tasks.Properties
{
    class Task1_InstagramPosts
    {
        public class Post
        {
            public string Caption { get; set; }
            public int Likes { get; set; }

            public Post(string caption, int likes)
            {
                Caption = caption;
                Likes = likes;
            }
        }
        public static void Run()
        {
            Console.Write("Enter number of users: ");
            if (!int.TryParse(Console.ReadLine(), out int userCount) || userCount <= 0)
            {
                Console.WriteLine("Invalid number of users.");
                return;
            }

            Post[][] instagramData = new Post[userCount][];

            for (int i = 0; i < userCount; i++)
            {
                Console.Write($"User {i + 1}: How many posts? ");
                if (!int.TryParse(Console.ReadLine(), out int postCount) || postCount < 0)
                {
                    Console.WriteLine("Invalid number of posts.");
                    return;
                }

                instagramData[i] = new Post[postCount];

                for (int j = 0; j < postCount; j++)
                {
                    Console.Write($"Enter caption for post {j + 1}: ");
                    string? caption = Console.ReadLine();

                    Console.Write("Enter likes: ");
                    if (!int.TryParse(Console.ReadLine(), out int likes) || likes < 0)
                    {
                        Console.WriteLine("Invalid number of likes.");
                        return;
                    }

                    instagramData[i][j] = new Post(caption ?? "", likes);
                }
            }

            Console.WriteLine("\n--- Displaying Instagram Posts ---");
            for (int i = 0; i < instagramData.Length; i++)
            {
                Console.WriteLine($"User {i + 1}:");
                for (int j = 0; j < instagramData[i].Length; j++)
                {
                    var post = instagramData[i][j];
                    Console.WriteLine($"Post {j + 1} - Caption: {post.Caption} | Likes: {post.Likes}");
                }
                Console.WriteLine();
            }
        }
    }
}
