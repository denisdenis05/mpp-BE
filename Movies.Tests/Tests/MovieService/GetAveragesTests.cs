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

public class GetAveragesTests
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
                    Genre = "Drama", Rating = 3
                }
                ,new Movie
                {
                    Id = 2, Title = "test2", Writer = "Stephen King", Director = "Frank Darabont", MPA = "R",
                    Genre = "Drama", Rating = 5
                }
                ,new Movie
                {
                    Id = 3, Title = "test3", Writer = "Stephen King", Director = "Frank Darabont", MPA = "R",
                    Genre = "Drama", Rating = 10
                }
            };
            
            var mockDbContext = new Mock<MovieDbContext>(new DbContextOptions<MovieDbContext>());
            mockDbContext.Setup(c => c.Movies).ReturnsDbSet(allMovies);
            
            return mockDbContext;
        }
        
        
        [Test]
        public async Task GetAverages_ShouldReturnMinMaxAndAverageRating()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAverages();

            // Assert: Verify the result matches expectations
            
            result.Min.Should().Be(3);
            result.Max.Should().Be(10);
            result.Average.Should().Be(6);
        }
        
    }
