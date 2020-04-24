using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Domain;
using Movies.Domain.Models;
using Movies.Infrastructure.Data;
using Movies.Shared.Interfaces;
using Movies.Web.ViewModels;

namespace Movies.Web.Controllers
{
    public class RentalController : Controller
    {
        private readonly IRepository _repository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly MoviesDbContext _context;
        public RentalController(IRepository repository, UserManager<ApplicationUser> userManager, MoviesDbContext context)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
        }


        public IActionResult Index(int movieID)
        {            
            ViewData["movieID"] = movieID;
            return View();
        }

        public async Task<ActionResult> ConfirmPaymentAsync([FromForm] Payment payment, int id)
        {
            string idString = Request.Form["MovieID"];
            int idInt = Int32.Parse(idString);
            var movie = _repository.GetById<Movie>(idInt);
            if (!String.IsNullOrEmpty(payment.CardNo))
            {
                if (ModelState.IsValid)
                {
                    if (payment.CheckLuhn())
                    {
                        //var paymentToRegister = new Payment()
                        //{
                        //    CardNo = payment.CardNo,
                        //    User = await _userManager.GetUserAsync(HttpContext.User),

                        //};
                        var User = await _userManager.GetUserAsync(HttpContext.User);
                        var paymentToRegister = new Payment();
                        paymentToRegister.SetCardNo(payment.CardNo);
                        paymentToRegister.SetPrice(movie.Price);
                        
                        _repository.Add(paymentToRegister);

                        //var rentToRegister = new Rental()
                        //{
                        //    MovieID = idInt,
                        //    Payment = paymentToRegister,
                        //    Price = movie.Price,
                        //    RentStartDate = DateTime.Now,
                        //    RentEndDate = DateTime.Now.AddDays(5),
                        //    User = await _userManager.GetUserAsync(HttpContext.User),
                        //};
                        var rentToRegister = new Rental();
                        rentToRegister.SetPayment(paymentToRegister);
                        rentToRegister.SetMovieId(movie.Id);
                        rentToRegister.SetStartDate();
                        rentToRegister.SetEndDate();
                        rentToRegister.SetUser(User);
                        rentToRegister.SetIsValid(true);


                        _repository.Add(rentToRegister);

                        
                        return View("Success", rentToRegister);
                    }
                }
            }
            ViewData["movieID"] = id;
            return View("Index");
        }

        public IActionResult Success()
        {
            return View();
        }

        public IActionResult RentedMovies()
        {
            var userName = HttpContext.User.Identity.Name;
            var userID = _userManager.GetUserId(User);

            var rentals = _repository.List<Rental>()
                .Where(r => r.UserId == userID && r.IsValid);

            return View(rentals);            
        }
        
    }
}