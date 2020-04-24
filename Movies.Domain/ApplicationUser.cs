using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Movies.Domain.Models;

namespace Movies.Domain
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public virtual List<Movie> FavouriteMovies { get; set; }
        public List<Movie> RatedMovies { get; set; }
        public List<BoardPost> BoardPosts { get; set; }

    }
}
    