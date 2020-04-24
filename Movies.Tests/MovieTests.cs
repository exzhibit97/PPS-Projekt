using Movies.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Movies.Tests
{
    public class MovieTests
    {
        [Fact]
        public void AddReview_ShouldAddReview()
        {
            //Arrange
            var movie = new Movie()
            {
                Title = "asd",
                Runtime = 1997,
                Year = 2000,
                PosterPath = null,
                Description = "Desc",
                Reviews = new List<Review>()
            };
            var review = new Review();
            //Act
            movie.AddReview(review);

            //Assert
            Assert.Single(movie.Reviews);
        }

        [Fact]
        public void AddReview_ShouldThrowArgumentNullException_DueToArgumentBeingNull()
        {
            //Arrange
            var movie = new Movie();

            //Act
            var ex = Assert.Throws<ArgumentNullException>(() => movie.AddReview(null));

            //Assert
            Assert.Equal("Value cannot be null.", ex.Message);
        }

        [Fact]
        public void DeleteReview_ShouldNotBeAbleToGetReview_DueToEmptyList()
        {
            //Arrange
            var movie = new Movie()
            {
                Title = "asd",
                Runtime = 1997,
                Year = 2000,
                PosterPath = null,
                Description = "Desc",
                Reviews = new List<Review>()
            };
            var review = new Review();

            //Act
            var ex = Assert.Throws<InvalidOperationException>(() => review = movie.Reviews.First());

            //Assert
            Assert.Equal("Sequence contains no elements", ex.Message);

        }

        [Fact]
        public void DeleteReview_ShouldNotBeAbleToRemoveReview_DueToEmptyList()
        {
            //Arrange
            var movie = new Movie()
            {
                Title = "asd",
                Runtime = 1997,
                Year = 2000,
                PosterPath = null,
                Description = "Desc",
                Reviews = new List<Review>()
            };
            var review = new Review();

            //Act
            var ex2 = Assert.Throws<InvalidOperationException>(() => movie.DeleteReview(movie.Reviews.First().Id));

            //Assert
            Assert.Equal("Sequence contains no elements", ex2.Message);
        }

        [Fact]
        public void SetDiscount_PriceDiffersFromBase()
        {
            //Arrange
            var movie = new Movie();
            movie.SetPrice(10);

            //Act
            movie.SetDiscount(40);

            //Assert
            Assert.NotEqual(10, movie.Price);
        }

        [Fact]
        public void IsRentPriceEqualToMoviePrice()
        {
            //Arrange
            var payment = new Payment();
            var movie = new Movie();
            movie.SetPrice(10);

            //Act
            payment.SetPrice(movie.Price);

            //Assert
            Assert.Equal(movie.Price, payment.Price);
        }

        [Fact]
        public void IsRentPriceEqualToMovie_DiscountedPrice()
        {
            //Arrange
            var payment = new Payment();
            var movie = new Movie();
            movie.SetPrice(10);
            movie.SetDiscount(50);

            //Act
            payment.SetPrice(movie.Price);

            //Assert
            Assert.Equal(movie.Price, payment.Price);
        }

        [Fact]
        public void DoTwoDifferentMoviesReallyDiffer_Equals()
        {           
            var movie1 = new Movie("Joker", 120, 2019, "", "Movie", 3.99M, new List<Review>());
            var movie2 = new Movie("Joker", 120, 2019, "", "Movie", 3.99M, new List<Review>());

            Assert.False(movie1.Equals(movie2));            
        }

        [Fact]
        public void DoTwoDifferentMoviesReallyDiffer_GetHashCode()
        {            
            var movie1 = new Movie("Joker", 120, 2019, "", "Movie", 3.99M, new List<Review>());
            var movie2 = new Movie("Joker", 120, 2019, "", "Movie", 3.99M, new List<Review>());

            Assert.Equal(movie1.GetHashCode(), movie2.GetHashCode());            
        }

    }
}
