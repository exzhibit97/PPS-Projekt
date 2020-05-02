using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Movies.Domain;
using Movies.Domain.Models;
using Movies.Infrastructure.Data;
using Movies.Shared.Interfaces;
using Movies.Web.ViewModels.SocialViewModels;
using Microsoft.EntityFrameworkCore;


namespace Movies.Web.Controllers
{
    public class SocialController : Controller
    {

        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MoviesDbContext _context;

        public SocialController(IRepository repository, UserManager<ApplicationUser> userManager, MoviesDbContext context)
        {
            _repository = repository;
            _userManager = userManager;
            _context = context;
        }


        [AllowAnonymous]
        [Route("Social/UserDetails/{UserId}")]
        public async Task<IActionResult> UserDetailsAsync(string UserId)
        {
            UserProfileViewModel userProfileViewModel = new UserProfileViewModel();
            userProfileViewModel.User = await _userManager.FindByIdAsync(UserId);
            userProfileViewModel.UserRatedMovies = _repository.List<Movie>().Take(5).ToList();

            return View(userProfileViewModel);
        }

        [Route("Social/Latest")]
        public IActionResult LatestFeed()
        {
            return View();
        }

        [Route("Social/FollowUser/{UserId}")]
        public async Task<IActionResult> FollowUser(string UserId)
        {
            var followedUser = await _userManager.FindByIdAsync(UserId);
            var followingUser = await _userManager.GetUserAsync(HttpContext.User);

            var currentFollowing = _repository.List<Following>().Where(f => f.FollowedUserId == UserId).Where(f => f.FollowingUserId == followingUser.Id).FirstOrDefault();

            if (currentFollowing != null)
            {
                if (currentFollowing.IsActive)
                {
                    currentFollowing.IsActive = false;
                    currentFollowing.EndedAt = DateTime.Now;
                    _repository.Update<Following>(currentFollowing);
                    return new JsonResult(currentFollowing);
                }
                else
                {
                    currentFollowing.IsActive = true;
                    currentFollowing.CreatedAt = DateTime.Now;
                    _repository.Update<Following>(currentFollowing);
                    return new JsonResult(currentFollowing);

                }
            }
            else
            {
                Following following = new Following();
                following.CreatedAt = DateTime.Now;
                following.FollowedUser = followedUser;
                following.FollowingUser = followingUser;
                following.IsActive = true;
                _repository.Add<Following>(following);
                return new JsonResult(following);
            }

        }

        [Route("Social/LikeMovie/{MovieID}")]
        public async Task<IActionResult> LikeMovie(int MovieID)
        {
            FavoritedMovie favoritedMovie = new FavoritedMovie();
            favoritedMovie.MovieID = MovieID;
            favoritedMovie.User = await _userManager.GetUserAsync(HttpContext.User);            
            favoritedMovie.CreatedAt = DateTime.Now;
            favoritedMovie.IsActive = true;

            _repository.Add<FavoritedMovie>(favoritedMovie);
            
            return RedirectToAction("Index", "Movies");
        }


        public async Task<IActionResult> PostOnBoard([FromBody] BoardPost post)
        {
            var user = await _userManager.GetUserAsync(User);
            BoardPost boardPost = new BoardPost()
            {
                Title = "placeholder",
                Content = post.Content,
                UserID = user.Id,                
                CreatedAt = DateTime.Now
            };

            _repository.Add<BoardPost>(boardPost);

            //return RedirectToAction("Index", "Home");
            return new JsonResult(boardPost);
        }

        public async Task<IActionResult> ReplyToPost([FromBody] Reply reply)
        {
            var user = await _userManager.GetUserAsync(User);
            Reply replyToAdd = new Reply()
            {
                Content = reply.Content,
                RootPostId = reply.RootPostId,
                UserID = user.Id
            };

            _repository.Add<Reply>(replyToAdd);

            return new JsonResult(replyToAdd);
        }        

        [Route("Social/PostDetails/{postId}")]
        public async Task<IActionResult> PostDetails(int postId)
        {
            
            var post = _context.BoardPosts.Include(p => p.Replies).Include(p => p.User).Where(p => p.Id == postId).Single();

            foreach(var reply in post.Replies)
            {
                reply.User = await _userManager.FindByIdAsync(reply.UserID);
            }

            return View(post);            
        }
    }
}