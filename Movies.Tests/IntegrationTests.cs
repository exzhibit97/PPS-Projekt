using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Movies.Domain.Models;
using Movies.Infrastructure.Data;
using Movies.Shared.Interfaces;
using Movies.Web.Controllers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Movies.Tests
{
    public class IntegrationTests : TestBase
    {

        [Fact]
        public void DoesNewAddedMovieReallyExistInDb()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieContext;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            var movieToAdd = new Movie()
            {
                Title = "asd",
                Runtime = 1997,
                Year = 2000,
                PosterPath = null,
                Description = "Desc",
                Reviews = new List<Review>()
            };

            var context = new MoviesDbContext(options);
            var last = context.Movies.ToList().Last();
            var repo = new EfRepository(context);

            //Act
            repo.Add(movieToAdd);

            
            //context.Add(movieToAdd);
            //context.SaveChanges();

            //Assert
            Assert.Equal(1997, last.Runtime);
        }

        [Fact]
        public void TestIfReviewAddedCorrectly()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieContext;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            var movieToAdd = new Movie()
            {
                Title = "asd",
                Runtime = 1997,
                Year = 2000,
                PosterPath = null,
                Description = "Desc"
            };

            var context = new MoviesDbContext(options);
            var last = context.Movies.ToList().Last();
            var repo = new EfRepository(context);

            //Act
            var movie = repo.List<Movie>().Last();

            var review = new Review()
            {
                Content = "review",
                Movie = movie
            };

            movie.Reviews.Add(review);
            var lastRev = movie.Reviews.Last();

            //Assert
            Assert.Equal("review", lastRev.Content);

        }

        [Fact]
        public void TestMovieDelete()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieContext;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            var movieToAdd = new Movie()
            {
                Title = "beforeLast",
                Runtime = 1997,
                Year = 2000,
                PosterPath = null,
                Description = "Desc"
            };

            var movieToDelete = new Movie()
            {
                Title = "lastToDelete",
                Runtime = 1997,
                Year = 2000,
                PosterPath = null,
                Description = "Desc"
            };



            var context = new MoviesDbContext(options);
            var repo = new EfRepository(context);

            //Act
            repo.Add(movieToAdd);
            repo.Add(movieToDelete);

            repo.Delete(movieToDelete);
            var last = context.Movies.ToList().Last();

            //Assert
            Assert.Equal(movieToAdd.Title, last.Title);
        }

        [Fact]
        public void TestMovieUpdate()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<MoviesDbContext>().EnableSensitiveDataLogging()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieContext;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            var movie = new Movie()
            {
                Title = "movieToEdit",
                Runtime = 1997,
                Year = 2000,
                PosterPath = null,
                Description = "Desc"
            };

            var context = new MoviesDbContext(options);
            var repo = new EfRepository(context);

            //Act
            repo.Add(movie);

            movie.Title = "edited";
            repo.Update<Movie>(movie);

            //Assert
            Assert.Equal("edited", movie.Title);
        }

        [Fact]
        public void RentalExpiredTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieContext;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;
            var context = new MoviesDbContext(options);
            var repo = new EfRepository(context);


            //Act
            var movie = new Movie() { Price = 3.99M };
            repo.Add<Movie>(movie);


            var payment = new Payment() { Price = movie.Price };
            repo.Add<Payment>(payment);
            var rental = new Rental();
            rental.SetStartDate();
            rental.SetEndDate(DateTime.Now.AddSeconds(2));
            rental.SetPayment(payment);
            rental.SetMovieId(movie.Id);
            repo.Add<Rental>(rental);

            Thread.Sleep(3000);

            //Assert
            Assert.False(rental.IsRentalValid());
        }

        [Fact]
        public void RentalValidTest()
        {
            //Arrange
            var options = new DbContextOptionsBuilder<MoviesDbContext>()
                .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MovieContext;Trusted_Connection=True;MultipleActiveResultSets=true")
                .Options;

            var context = new MoviesDbContext(options);
            var repo = new EfRepository(context);

            //Act
            var movie = new Movie() { Price = 3.99M };
            repo.Add<Movie>(movie);


            var payment = new Payment() { Price = movie.Price };
            repo.Add<Payment>(payment);
            var rental = new Rental();
            rental.SetStartDate();
            rental.SetEndDate(DateTime.Now.AddSeconds(20));
            rental.SetPayment(payment);
            rental.SetMovieId(movie.Id);
            repo.Add<Rental>(rental);

            //Assert
            Assert.True(rental.IsRentalValid());
        }
    }
}
