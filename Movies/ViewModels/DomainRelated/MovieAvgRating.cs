using Movies.Domain.Models;

namespace Movies.Web.ViewModels.DomainRelated
{
    public class MovieAvgRating
    {

        public Movie Movie { get; set; }
        public double Rating { get; set; }
    }
}
