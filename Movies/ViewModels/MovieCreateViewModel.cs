using Microsoft.AspNetCore.Http;
using Movies.Domain.Models;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace Movies.Web.ViewModels
{
    public class MovieCreateViewModel
    {
        public int Id { get; set; }
        [StringLength(60, MinimumLength = 1)]
        [Required]
        public string Title { get; set; }
        [Range(1, 9999)]
        [Required]
        public int Runtime { get; set; }
        [Range(1900, 2030)]
        [Required]
        public int Year { get; set; }
        public IFormFile Poster { get; set; }
        [StringLength(300, MinimumLength = 1)]
        [Required]
        public string Description { get; set; }

        public static MovieCreateViewModel FromMovie(Movie movie)
        {
            var stream = File.OpenRead("C:/Users/Patryk/source/repos/Movies/Movies/wwwroot/images/poster-placeholder.png");
            return new MovieCreateViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                Runtime = movie.Runtime,
                Year = movie.Year,
                Poster = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name)),
                Description = movie.Description
            };
        }
    }
}
