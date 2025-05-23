using Bogus;
using Movies.Data;
using Microsoft.EntityFrameworkCore;
using Movies.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;

// Database connection configuration
var options = new DbContextOptionsBuilder<MovieDbContext>()
    .UseNpgsql("Host=localhost;Port=5432;Database=Movies;Username=postgres;Password=")
    .Options;

// Define movie genres and MPA ratings for more realistic data
var genres = new[] 
{ 
    "Action", "Adventure", "Animation", "Biography", "Comedy", 
    "Crime", "Documentary", "Drama", "Family", "Fantasy", 
    "History", "Horror", "Musical", "Mystery", "Romance", 
    "Sci-Fi", "Sport", "Thriller", "War", "Western" 
};

var mpaRatings = new[] { "G", "PG", "PG-13", "R", "NC-17" };

// Create endorsement faker
var endorsementId = 1;
var endorsementFaker = new Faker<Endorsement>()
    .RuleFor(e => e.EndorsementId, f => endorsementId++)
    .RuleFor(e => e.Endorser, f => f.Company.CompanyName())
    .RuleFor(e => e.Date, f => DateTime.SpecifyKind(f.Date.Past(3), DateTimeKind.Utc)); // Endorsements from the past 3 years in UTC

// Movie faker with string writer and endorsements collection
int movieId = 1;
var movieFaker = new Faker<Movie>()
    .RuleFor(m => m.Id, f => movieId++)
    .RuleFor(m => m.Title, f => f.Commerce.ProductName())
    .RuleFor(m => m.Director, f => f.Name.FullName())
    .RuleFor(m => m.Writer, f => f.Name.FullName()) // Single writer as string
    .RuleFor(m => m.Genre, f => f.PickRandom(genres))
    .RuleFor(m => m.MPA, f => f.PickRandom(mpaRatings))
    .RuleFor(m => m.Rating, f => Math.Round(f.Random.Double(1, 10), 1))
    .RuleFor(m => m.Endorsements, f => endorsementFaker.Generate(f.Random.Int(0, 5)).ToList()); // 0-5 endorsements per movie

// Batch size for efficient saving
const int batchSize = 1000;
const int totalMovies = 100000;
int totalBatches = (int)Math.Ceiling((double)totalMovies / batchSize);

Console.WriteLine($"Starting to generate {totalMovies} movies in {totalBatches} batches...");

using (var dbContext = new MovieDbContext(options))
{
    // Ensure database is created with correct schema
    dbContext.Database.EnsureCreated();
    
    // Clear existing data if needed
    dbContext.Movies.RemoveRange(dbContext.Movies);
    
    // The Writer table no longer exists, so we remove this line
    // dbContext.Writers.RemoveRange(dbContext.Writers);
    
    // Check if Endorsements table exists and clear if needed
    if (dbContext.Set<Endorsement>().Any())
    {
        dbContext.Set<Endorsement>().RemoveRange(dbContext.Set<Endorsement>());
    }
    
    dbContext.SaveChanges();
    
    // Create and save movies in batches
    for (int batch = 0; batch < totalBatches; batch++)
    {
        int remaining = Math.Min(batchSize, totalMovies - (batch * batchSize));
        Console.WriteLine($"Generating batch {batch + 1}/{totalBatches} ({remaining} movies)...");
        
        var movies = movieFaker.Generate(remaining);
        
        dbContext.Movies.AddRange(movies);
        dbContext.SaveChanges();
        
        Console.WriteLine($"Saved batch {batch + 1}. Total progress: {Math.Min((batch + 1) * batchSize, totalMovies)}/{totalMovies}");
    }
}

Console.WriteLine("Data generation complete!");
Console.WriteLine($"Added {totalMovies} movies with endorsements to the database.");