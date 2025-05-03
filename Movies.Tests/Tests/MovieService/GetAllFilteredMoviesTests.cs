using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using Moq.EntityFrameworkCore;
using Movies.Business.Models.Movies;
using Movies.Business.Models.PagingAndFiltering;
using Movies.Business.Services.Filtering;
using Movies.Data;
using Movies.Data.Models;
using NUnit.Framework;

namespace TestProject1.Tests.MovieService;

public class GetAllFilteredMoviesTests
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
    
            mockDbContext.Setup(c => c.Movies).ReturnsDbSet(allMovies.AsQueryable());
    
            mockDbContext.Setup(c => c.Movies.FindAsync(It.IsAny<object[]>()))
                .ReturnsAsync((object[] ids) => allMovies.FirstOrDefault(m => m.Id == (int)ids[0]));
    
            mockDbContext.Setup(c => c.SaveChanges()).Returns(1);
            mockDbContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
    
            return mockDbContext;
        }

        private FilteringDTO constructFilterRequest(string field, string value, string operation) =>
            new FilteringDTO
            {
                FieldToFilterBy = field,
                Value = value,
                Operation = operation
            };
        
        private SortingDTO constructSortRequest(string field, string order) =>
            new SortingDTO()
            {
                FieldToSortBy = field,
                Order = order,
            };
        
        private PagingDTO constructPagingRequest(int page, int elements) =>
            new PagingDTO()
            {
                PageNumber = page,
                PageSize = elements
            };

        private FilterObjectDTO constructEmptyFilterObjectRequest()
        {
            var request = new FilterObjectDTO();
            request.Filtering = new List<FilteringDTO>();
            request.Sorting = constructSortRequest("","");
            request.Paging = constructPagingRequest(1,5);
            
            return request;
        }
        
        private FilterObjectDTO constructPaginatedFilterObjectRequest()
        {
            var request = new FilterObjectDTO();
            request.Filtering = new List<FilteringDTO>();
            request.Sorting = constructSortRequest("","");
            request.Paging = constructPagingRequest(1,1);
            
            return request;
        }
        
        private FilterObjectDTO constructSortDescendingFilterObjectRequest()
        {
            var request = new FilterObjectDTO();
            request.Filtering = new List<FilteringDTO>();
            request.Sorting = constructSortRequest("Id","desc");
            request.Paging = constructPagingRequest(1,5);
            
            return request;
        }
        private FilterObjectDTO constructSortAscendingFilterObjectRequest()
        {
            var request = new FilterObjectDTO();
            request.Filtering = new List<FilteringDTO>();
            request.Sorting = constructSortRequest("Id","asc");
            request.Paging = constructPagingRequest(1,5);
            
            return request;
        }
        
        private FilterObjectDTO constructEqualFilterObjectRequest()
        {
            var request = new FilterObjectDTO();
            request.Filtering = new List<FilteringDTO>();
            request.Filtering.Add(constructFilterRequest("Title", "test2", "eq"));
            request.Sorting = constructSortRequest("","");
            request.Paging = constructPagingRequest(1,5);
            
            return request;
        }
        
        private FilterObjectDTO constructInFilterObjectRequest()
        {
            var request = new FilterObjectDTO();
            request.Filtering = new List<FilteringDTO>();
            request.Filtering.Add(constructFilterRequest("Title", "test", "in"));
            request.Sorting = constructSortRequest("","");
            request.Paging = constructPagingRequest(1,5);
            
            return request;
        }
        
        private FilterObjectDTO constructLessFilterObjectRequest()
        {
            var request = new FilterObjectDTO();
            request.Filtering = new List<FilteringDTO>();
            request.Filtering.Add(constructFilterRequest("Id", "3", "lt"));
            request.Sorting = constructSortRequest("","");
            request.Paging = constructPagingRequest(1,5);
            
            return request;
        }
        
        [Test]
        public async Task GetAllFilteredMoviesWithEmptyFilter_ShouldReturnAllMoviesPaginated()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAllFilteredMovies(constructEmptyFilterObjectRequest());

            // Assert: Verify the result matches expectations
            result.NumberRetrieved.Should().Be(3);
        }
        
        [Test]
        public async Task GetAllFilteredMoviesWithEmptyFilterPaginated_ShouldReturnOnly1Movie()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAllFilteredMovies(constructPaginatedFilterObjectRequest());

            // Assert: Verify the result matches expectations
            result.NumberRetrieved.Should().Be(1);
            result.NumberFound.Should().Be(3);
        }
        
        [Test]
        public async Task GetAllFilteredMoviesDescendingSortedById_ShouldReturnAllMoviesPaginatedAndSortedDescending()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAllFilteredMovies(constructSortDescendingFilterObjectRequest());

            // Assert: Verify the result matches expectations
            result.NumberRetrieved.Should().Be(3);
            result.Results.First().Id.Should().Be(3);
        }
        
        [Test]
        public async Task GetAllFilteredMoviesAscendingSortedById_ShouldReturnAllMoviesPaginatedAndSortedAscending()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAllFilteredMovies(constructSortAscendingFilterObjectRequest());

            // Assert: Verify the result matches expectations
            result.NumberRetrieved.Should().Be(3);
            result.Results.First().Id.Should().Be(1);
        }
        
        [Test]
        public async Task GetAllFilteredMoviesFilteredByExactTitle_ShouldReturnAllMoviesWithExactTitle()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAllFilteredMovies(constructEqualFilterObjectRequest());

            // Assert: Verify the result matches expectations
            result.NumberRetrieved.Should().Be(1);
            result.Results.First().Id.Should().Be(2);
        }
        
        [Test]
        public async Task GetAllFilteredMoviesFilteredByInTitle_ShouldReturnAllMoviesWithTitleSimilar()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAllFilteredMovies(constructInFilterObjectRequest());

            // Assert: Verify the result matches expectations
            result.NumberRetrieved.Should().Be(3);
            result.Results.First().Id.Should().Be(1);
        }
        
        [Test]
        public async Task GetAllFilteredMoviesFilteredByLowerThanId_ShouldReturnAllMoviesWithIdLower()
        {
            // Act: Call the method being tested
            var result = await _movieService.GetAllFilteredMovies(constructLessFilterObjectRequest());

            // Assert: Verify the result matches expectations
            result.NumberRetrieved.Should().Be(2);
            result.Results.Last().Id.Should().Be(2);
        }
    }
