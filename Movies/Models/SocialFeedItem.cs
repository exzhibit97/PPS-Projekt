using Movies.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Web.Models
{
    public class SocialFeedItem
    {
        //Type = "review", Title = movies.Title, Rating = reviews.Rating, Content = reviews.Content, User = followings.FollowedUser, CreatedAt = reviews.CreatedAt
        public int ItemId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public int Rating{ get; set; }
        public string Content { get; set; }
        public ApplicationUser User{ get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
