using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Movies.Domain.Models;
using Movies.Shared.Interfaces;
using Movies.Web.Models;
using Movies.Web.ViewModels;
using Rotativa.AspNetCore;
using Rotativa;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Movies.Web.Controllers
{
    [Authorize]
    public class MoviesController : Controller
    {
        private readonly IRepository _repository;
        private readonly IWebHostEnvironment iHostingEnvironment;

        public MoviesController(IRepository repository, IWebHostEnvironment IHostingEnvironment)
        {
            _repository = repository;
            iHostingEnvironment = IHostingEnvironment;
        }


        //// GET: Movies
        //[AllowAnonymous]
        //public ViewResult Home()
        //{
        //    var movies = _repository.List<Movie>();
        //    //.Select(MovieDTO.FromMovie);
        //    return View(movies);
        //}
        [AllowAnonymous]
        public ViewResult Index(string sortOrder, string searchString)
        {
            ViewBag.NameSortParm = sortOrder == "Title" ? "title_desc" : "Title";
            ViewBag.YearSortParm = sortOrder == "Year" ? "year_desc" : "Year";
            ViewBag.RuntimeSortParm = sortOrder == "Runtime" ? "runtime_desc" : "Runtime";
            var movies = _repository.List<Movie>()
                           .Select(MovieDTO.FromMovie);
            if (!String.IsNullOrEmpty(searchString))
            {
                movies = movies.Where(m => m.Title.Contains(searchString));
                //if (movies.Count() == 0)
                //{
                //    return PartialView("Partials/_SearchStringNotFound");
                //}
            }
            switch (sortOrder)
            {
                case "Title":
                    movies = movies.OrderBy(m => m.Title);
                    break;
                case "title_desc":
                    movies = movies.OrderByDescending(m => m.Title);
                    break;
                case "Year":
                    movies = movies.OrderBy(m => m.Year);
                    break;
                case "year_desc":
                    movies = movies.OrderByDescending(m => m.Year);
                    break;
                case "Runtime":
                    movies = movies.OrderBy(m => m.Runtime);
                    break;
                case "runtime_desc":
                    movies = movies.OrderByDescending(m => m.Runtime);
                    break;
                default:
                    movies = movies.OrderBy(m => m.Id);
                    break;
            }
            MoviesController controller = new MoviesController(_repository, iHostingEnvironment);
            return View(movies);
        }

        public async Task<ActionResult> MoviesToPdf()
        {
            var movies = _repository.List<Movie>()
                           .Select(MovieDTO.FromMovie).Take(1);
            //return new ViewAsPdf("Index", movies);
            var rpt = new Rotativa.AspNetCore.ViewAsPdf();
            //rptLandscape.Model = Model;
            rpt.PageOrientation = Rotativa.AspNetCore.Options.Orientation.Portrait;
            rpt.PageSize = Rotativa.AspNetCore.Options.Size.A4;
            rpt.ViewName = "Index";
            rpt.Model = movies;
            var context = this.ControllerContext;
            byte[] arr = await rpt.BuildFile(context);
            System.IO.File.WriteAllBytes(Path.Combine(iHostingEnvironment.WebRootPath, "PDFStorage", "File" + Guid.NewGuid() + ".pdf"), arr);
            Console.WriteLine("ASDASDASDASD" + this.ControllerContext.HttpContext.RequestServices.GetService(typeof(ITempDataProvider)).ToString() + "ASDASDASDASDASDASDASDDS");
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public ActionResult Details(int id)
        {
            //if (id == null)
            //{
            //    return NotFound();
            //}
            var movie = _repository.GetById<Movie>(id);
            if (movie == null)
            {
                return NotFound();
            }
            //var movieToShow = new MovieDTO()
            //{
            //    Id = movie.Id,
            //    Title = movie.Title,
            //    Runtime = movie.Runtime,
            //    Description = movie.Description,
            //    PosterPath = movie.PosterPath,
            //    Year = movie.Year,
            //    Reviews = movie.Reviews
            //};
            var movieToShow = MovieDTO.FromMovie(movie);

            return View(movieToShow);
        }

        // GET: Movies/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([Bind("MovieId,Title,Runtime,Year,Poster,Description")] MovieCreateViewModel movie)
        {
            if (!ModelState.IsValid)
            {
                return View(movie);
            }
            string uniqueFileName = null;
            if (movie.Poster != null)
            {
                string uploadsFolder = Path.Combine(iHostingEnvironment.WebRootPath, "images");
                string fileName = movie.Poster.FileName;
                uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                movie.Poster.CopyTo(new FileStream(filePath, FileMode.Create));
            }
            var movieToAdd = new Movie()
            {
                Title = movie.Title,
                Runtime = movie.Runtime,
                Year = movie.Year,
                PosterPath = uniqueFileName,
                Description = movie.Description,
            };
            _repository.Add(movieToAdd);
            return RedirectToAction(nameof(Index), "Movies");
        }

        // GET: Movies/Edit/5
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _repository.GetById<Movie>((int)id);
            if (movie == null)
            {
                return NotFound();
            }
            var movieToUpdate = MovieCreateViewModel.FromMovie(movie);
            return View(movieToUpdate);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id, [Bind("Id,Title,Runtime,Year,Poster,Description")] MovieCreateViewModel movie)
        {
            //if (id != movie.Id)
            //{
            //    return NotFound();
            //}

            if (ModelState.IsValid)
            {
                try
                {

                    string uniqueFileName = null;
                    if (movie.Poster != null)
                    {
                        string uploadsFolder = Path.Combine(iHostingEnvironment.WebRootPath, "images");
                        string fileName = movie.Poster.FileName;
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        movie.Poster.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                    var movieToCheckPoster = _repository.GetById<Movie>(id);
                    if (movieToCheckPoster.PosterPath != null)
                    {
                        uniqueFileName = movieToCheckPoster.PosterPath;
                        var movieToUpdate = new Movie()
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            Runtime = movie.Runtime,
                            Year = movie.Year,
                            PosterPath = uniqueFileName,
                            Description = movie.Description,
                        };
                        _repository.Update(movieToUpdate);
                    }
                    else
                    {
                        var movieToUpdate = new Movie()
                        {
                            Id = movie.Id,
                            Title = movie.Title,
                            Runtime = movie.Runtime,
                            Year = movie.Year,
                            PosterPath = uniqueFileName,
                            Description = movie.Description,
                        };
                        _repository.Update(movieToUpdate);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_repository.List<Movie>().Any(m => m.Id == id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(movie);
        }

        // GET: Movies/Delete/5
        public IActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var movie = _repository.GetById<Movie>((int)id);

            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var movie = _repository.GetById<Movie>(id);
            _repository.Delete<Movie>(movie);

            return RedirectToAction(nameof(Index));
        }

        public void CheckMovies()
        {
            var movies = _repository.List<Movie>();

            foreach (var movie in movies)
            {
                if (movie.Title.Contains("234"))
                {
                    movie.Title = "To remove";
                    _repository.Update(movie);
                }

            }
        }

    }
}
