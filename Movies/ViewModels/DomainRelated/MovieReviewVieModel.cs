using Movies.Domain.Models;

namespace Movies.Web.ViewModels.DomainRelated
{
    public class MovieReviewVieModel
    {
        public Movie Movie { get; set; }
        public Review Review { get; set; }
    }
}
