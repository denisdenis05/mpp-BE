using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Business.Services.Filtering;
using Movies.Data;
using Movies.Data.Models;
using NUnit.Framework;

namespace TestProject1.Tests.MovieService;

public class EditMovieTests
    {
        private Mock<MovieDbContext> _mockDbContext;
        private Movies.Business.Services.Movies.MovieService _movieService;
        private FilterService<Movie, MovieResponse> _filterService;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = SetupDbContext();

            _filterService = new FilterService<Movie, MovieResponse>(p => p.toMovieResponse());
            _movieService = new Movies.Business.Services.Movies.MovieService(_filterService, _mockDbContext.Object);
        }

        private Mock<MovieDbContext> SetupDbContext()
        {
            var allMovies = new List<Movie>
            {
                new Movie
                {
                    Id = 1, Title = "test1", Writer = "Stephen King", Director = "Frank Darabont", MPA = "R",
                    Genre = "Drama", Rating = 9.3
                }
                ,new Movie
                {
                    Id = 2, Title = "test2", Writer = "Stephen King", Director = "Frank Darabont", MPA = "R",
                    Genre = "Drama", Rating = 9.3
                }
                ,new Movie
                {
                    Id = 3, Title = "test3", Writer = "Stephen King", Director = "Frank Darabont", MPA = "R",
                    Genre = "Drama", Rating = 9.3
                }
            };
            
            var mockDbContext = new Mock<MovieDbContext>(new DbContextOptions<MovieDbContext>());
            mockDbContext.Setup(c => c.Movies).ReturnsDbSet(allMovies);
            
            return mockDbContext;
        }
        
        
        [Test]
        public async Task AfterEditGetAllMovies_ShouldReturnAllMoviesAndEditedOne()
        {
            // Act: Call the method being tested
            await _movieService.EditMovie(new EditMovieDTO(){Id = 1, Director = "TEST", Genre = "TEST", Rating = 1, Title = "TEST", Writer = "TEST", MPA = "TEST"});
            var result = await _movieService.GetAllMovies();

            // Assert: Verify the result matches expectations
            result.Count.Should().Be(3);
            result.First().Director.Should().Be("TEST");
        }
        
    }
