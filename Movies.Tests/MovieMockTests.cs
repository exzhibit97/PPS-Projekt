using Microsoft.AspNetCore.Mvc;
using Moq;
using Movies.Domain.Models;
using Movies.Shared.Interfaces;
using Movies.Web.Controllers;
using Movies.Web.Models;
using Movies.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Movies.Tests
{
    public class MovieMockTests : TestBase
    {
        [Fact]
        public void Index_ReturnsAViewResult_WithASearchStringFilteredMovieSet()
        {
            //Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.List<MovieDTO>()).Returns(GetTestMovies());
            var controller = new MoviesController(mock.Object, webHostEnvironment);

            //Act
            var result = controller.Index("Title", "");

            //Assert
            var model = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(result.ViewData.Model);
            Assert.Equal(2, model.Count());
            //Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ReturnsAViewResult_WithAListOfMovieSet()
        {
            //Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.List<MovieDTO>()).Returns(GetTestMovies());
            var controller = new MoviesController(mock.Object, webHostEnvironment);

            //Act
            var result = controller.Index(null, null);

            //Assert
            var model = Assert.IsAssignableFrom<IEnumerable<MovieDTO>>(result.ViewData.Model);
            Assert.Equal(2, model.Count());
            //Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Create_ReturnsBadRequestResult_WhenModelStateIsInvalid()
        {
            // Arrange
            var mock = new Mock<IRepository>();
            mock.Setup(repo => repo.List<MovieDTO>()).Returns(GetTestMovies());
            var controller = new MoviesController(mock.Object, webHostEnvironment);
            controller.ModelState.AddModelError("Title", "Required");
            var newMovie = new MovieCreateViewModel();

            // Act
            var result = controller.Create(newMovie);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<ViewResult>(viewResult);
        }

        [Fact]
        public void Create_ReturnsARedirectAndAddsSession_WhenModelStateIsValid()
        {
            // Arrange
            var mockRepo = new Mock<IRepository>();
            mockRepo.Setup(repo => repo.Add<Movie>(It.IsAny<Movie>()))
                .Verifiable();
            var controller = new MoviesController(mockRepo.Object, webHostEnvironment);
            var newMovie = new MovieCreateViewModel()
            {
                Title = "New Movie"
            };

            // Act
            var result = controller.Create(newMovie);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Movies", redirectToActionResult.ControllerName);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            mockRepo.Verify();
        }

        private List<MovieDTO> GetTestMovies()
        {
            var sessions = new List<MovieDTO>();
            sessions.Add(new MovieDTO()
            {
                Title = "Joker",
                Runtime = 120
            });
            sessions.Add(new MovieDTO()
            {
                Title = "Batman",
                Runtime = 130
            });
            return sessions;
        }
    }
}
