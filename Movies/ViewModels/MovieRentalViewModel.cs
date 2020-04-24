using Movies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movies.Web.ViewModels
{
    public class MovieRentalViewModel
    {
        public Movie Movie { get; set; }
        public Rental Rental { get; set; }
        public Payment Payment { get; set; }
    }
}
