using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Movies.Infrastructure.Data;
using Movies.Shared.Interfaces;
using Movies.Web.Models;
using Movies.Domain.Models;
using Movies.Domain;

namespace Movies.Web.Controllers
{
    [Authorize]
    public class ReviewsController : Controller
    {
        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReviewsController(IRepository _repository, UserManager<ApplicationUser> _userManager)
        {
            this._repository = _repository;
            this._userManager = _userManager;
        }

        // GET: Reviews
        [Authorize]
        public ActionResult GetReviews(int Id)
        {
            var reviews = _repository.List<Review>()
               .Select(ReviewDTO.FromReview).Where(r => r.MovieId == Id);
            return View(reviews);
        }

        public async Task<ActionResult> PostReview([FromForm] ReviewDTO review)
        {
            var user = await _userManager.GetUserAsync(HttpContext.User);
            var userComment = _repository.List<Review>().Where(r => r.UserID == user.Id).Where(r => r.MovieId == review.MovieId).FirstOrDefault();
            if (ModelState.IsValid)
            {
                var reviewToAdd = new Review()
                {
                    Id = review.Id,
                    Content = review.Content,
                    Rating = review.Rating,
                    CreatedAt = review.PostedOn,
                    MovieId = review.MovieId,
                    Movie = review.Movie,
                    User = await _userManager.GetUserAsync(HttpContext.User)
                };
                if (userComment == null)
                {
                    _repository.Add<Review>(reviewToAdd);
                } else
                {
                    reviewToAdd.Id = userComment.Id;
                    _repository.Update<Review>(reviewToAdd);
                }


                return RedirectToAction("Details", "Movies", new { id = review.MovieId });
            }

            return RedirectToAction("Details", "Movies", new { id = review.MovieId });
        }
    }
}
