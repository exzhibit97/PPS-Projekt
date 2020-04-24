using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Movies.Infrastructure.Data;
using Movies.Shared.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Movies.Web.ViewComponents
{
    [ViewComponent]
    public class TopRatedViewComponent : ViewComponent
    {
        private readonly IRepository _repository;
        private readonly MoviesDbContext _context;

        public TopRatedViewComponent(IRepository repository, MoviesDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public IViewComponentResult Invoke()
        {            
            var movies = _context.Movies
                .Include(m => m.Reviews)
                .OrderByDescending(m => m.Reviews.Count)
                .Take(3)
                .ToList();

            return View(movies);
        }
    }
}
