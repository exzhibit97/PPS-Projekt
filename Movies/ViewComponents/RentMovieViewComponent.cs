using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Web.ViewComponents
{
    [ViewComponent]
    public class RentMovieViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int movieID)
        {
            ViewData["movieID"] = movieID;
            return View();
        }
    }
}
