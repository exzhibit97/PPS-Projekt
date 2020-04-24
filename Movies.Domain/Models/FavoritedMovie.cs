using Movies.Shared;
using Movies.Shared.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Movies.Domain.Models
{
    public class FavoritedMovie : BaseEntity, IFeed
    {
        public int MovieID { get; set; }
        [ForeignKey("MovieID")]
        public Movie Movie { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
