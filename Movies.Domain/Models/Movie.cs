using System;
using System.Collections.Generic;
using System.Text;
using Movies.Shared;

namespace Movies.Domain.Models
{
    public class Movie : BaseEntity, IEquatable<Movie>
    {        
        public string Title { get; set; }
        public int Runtime { get; set; }
        public int Year { get; set; }
        public string PosterPath { get; set; }
        public string Description { get; set; }

        public decimal Price { get; set; }

        public List<Review> Reviews { get; set; }

        public Movie(string title, int runtime, int year, string posterPath, string description, decimal price, List<Review> reviews)
        {
            Title = title;
            Runtime = runtime;
            Year = year;
            PosterPath = posterPath;
            Description = description;
            Price = price;
            Reviews = reviews;
        }

        public Movie()
        {
            Reviews = new List<Review>();
        }
               

        public void AddReview(Review review)
        {
            if (review == null)
            {
                throw new ArgumentNullException();
            }
            Reviews.Add(review);
        }

        public void DeleteReview(int id)
        {
            var reviewToDelete = Reviews.Find(r => r.Id == id);
            Reviews.Remove(reviewToDelete);
        }

        public void SetPrice(decimal price)
        {
            if (price < 0)
            {
                throw new ArgumentException();
            }
            this.Price = price;
        }


        public IEnumerable<Review> GetReviews() => Reviews;

        //added 01.05.2020
        public void SetDiscount(decimal discountRate)
        {
            decimal discount = (discountRate / 100) * this.Price;
            this.Price -= discount;
        } 
        public bool Equals(Movie other)
        {
            ////Check whether the compared object is null. 
            //if (Object.ReferenceEquals(other, null)) return false;

            ////Check whether the compared object references the same data. 
            //if (Object.ReferenceEquals(this, other)) return true;

            //Check whether the products' properties are equal. 
            return Title.Equals(other.Title);
        }
    }
}
