using Movies.Domain.Models;
using Movies.Shared;
using System.Collections.Generic;

namespace Movies.Web.Models
{
    public class MovieDTO : BaseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Runtime { get; set; }
        public int Year { get; set; }
        public string PosterPath { get; set; }
        public string Description { get; set; }

        public List<Review> Reviews { get; set; }

        public static MovieDTO FromMovie(Movie movie)
        {
            return new MovieDTO()
            {
                Id = movie.Id,
                Title = movie.Title,
                Runtime = movie.Runtime,
                Year = movie.Year,
                PosterPath = movie.PosterPath,
                Description = movie.Description,
                Reviews = movie.Reviews,
            };
        }
    }
}
