using Movies.Domain;
using Movies.Domain.Models;
using Movies.Infrastructure.Data;
using System;
using System.ComponentModel.DataAnnotations;

namespace Movies.Web.Models
{
    public class ReviewDTO
    {
        public int Id { get; set; }
        [StringLength(400, MinimumLength = 0)]
        public string Content { get; set; }
        [Range(0, 10)]
        public int Rating { get; set; }
        public DateTime PostedOn { get; set; } = DateTime.Now;
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public ApplicationUser User { get; set; }
        public string UserID { get; set; }
               
        public ReviewDTO()
        {

        }
        public static ReviewDTO FromReview(Review review)
        {
            return new ReviewDTO()
            {
                Id = review.Id,
                Content = review.Content,
                Rating = review.Rating,
                PostedOn = review.CreatedAt,
                MovieId = review.MovieId,
                Movie = review.Movie
            };
        }
    }
}
