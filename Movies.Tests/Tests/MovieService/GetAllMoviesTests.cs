using FluentAssertions;
using Moq;
using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Business.Services.Filtering;
using Movies.Data;
using Movies.Data.Models;
using NUnit.Framework;

namespace TestProject1.Tests.MovieService;

public class GetAllMoviesTests
    {
        private Mock<DbContext> _mockDbContext;
        private Movies.Business.Services.Movies.MovieService _movieService;
        private FilterService<Movie, MovieResponse> _filterService;

        [SetUp]
        public void Setup()
        {
            _mockDbContext = SetupDbContext();

            _filterService = new FilterService<Movie, MovieResponse>(p => p.toMovieResponse());
            _movieService = new Movies.Business.Services.Movies.MovieService(_filterService, _mockDbContext.Object);
        }

        private Mock<DbContext> SetupDbContext()
        {
            var dbContext = new Mock<DbContext>();
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
            
            dbContext.Object.Movies = allMovies;

            
            return dbContext;
        }
        
        
        [Test]
        public async Task GetAllMovies_ShouldReturnAllMovies()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAllMovies();

            // Assert: Verify the result matches expectations
            result.Count.Should().Be(3);
        }
        
    }
