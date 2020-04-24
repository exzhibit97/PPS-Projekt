using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Movies.Domain.Models;
using Movies.Infrastructure.Data;
using Movies.Models;
using Movies.Shared.Interfaces;
using Movies.Web.Models;
using Rotativa.AspNetCore;

namespace Movies.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRepository _repository;
        private readonly IWebHostEnvironment _iHostingEnvironment;
        private readonly MoviesDbContext _context;

        public HomeController(ILogger<HomeController> logger, IRepository repository, IWebHostEnvironment iHostingEnvironment, MoviesDbContext context)
        {
            _repository = repository;
            _logger = logger;
            _iHostingEnvironment = iHostingEnvironment;
            _context = context;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            //RecurringJob.AddOrUpdate(() => CheckMovies(), Cron.Hourly);
            //RecurringJob.AddOrUpdate(() => WriteToFile(), "59 23 * * *");
            return View();
        }

        [AllowAnonymous]
        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public void CheckMovies()
        {
            var rentals = _repository.List<Rental>();

            foreach (var rental in rentals)
            {
                if (rental.IsRentalValid())
                {
                    rental.SetIsValid(true);
                }
                else
                {
                    rental.SetIsValid(false);
                }
                _repository.Update(rental);

            }
        }

        public void WriteToFile()
        {
            var rentals = from rent in _context.Rentals
                          where rent.RentStartDate.Date == DateTime.Now.Date
                          group rent by rent.MovieID into movieID
                          select new MovieRentalGroup()
                          {
                              movieID = movieID.Key,
                              movieCount = movieID.Count()
                          };

            string[] lines = new string[20];
            int i = 1;
            decimal sumOfDayRevenue = 0M;
            lines[0] = new StringBuilder("Data generated for day:" + DateTime.Now.ToShortDateString()).ToString();
            if (rentals.Count() != 0)
            {
                foreach (var item in rentals)
                {
                    StringBuilder stringBuilder = new StringBuilder("");
                    Movie movie = _repository.GetById<Movie>(item.movieID);
                    if (movie != null) {
                        sumOfDayRevenue += movie.Price;
                        var count = item.movieCount;
                        stringBuilder.Append(movie.Title + " was rented by " + count + " users");
                    } else { sumOfDayRevenue += 0; }
                    lines[i] = stringBuilder.ToString();
                    i++;
                }
                lines[i] = "Todays revenue is " + sumOfDayRevenue + "$";
                // WriteAllLines creates a file, writes a collection of strings to the file,
                // and then closes the file.  You do NOT need to call Flush() or Close().            
                System.IO.File.WriteAllLines(Path.Combine(_iHostingEnvironment.WebRootPath, "reports", "File" + Guid.NewGuid() + ".txt"), lines);
            }
            else
            {

                lines[1] = new StringBuilder("No movies rented this day.").ToString();
                System.IO.File.WriteAllLines(Path.Combine(_iHostingEnvironment.WebRootPath, "reports", "File" + Guid.NewGuid() + ".txt"), lines);
            }
        }
        public class MovieRentalGroup
        {
            public int movieID { get; set; }
            public int movieCount { get; set; }
            public MovieRentalGroup()
            {

            }
        }
    }
}
