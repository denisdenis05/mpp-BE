//using Movies.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Movies.Data.Configurations;
using Movies.Data.Models;

namespace Movies.Data;

public class DbContext : Microsoft.EntityFrameworkCore.DbContext
{
    /*public DbContext(DbContextOptions<DbContext> options) : base(options)
    {
    }

    public DbSet<Test> Examples { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TestConfiguration());
        base.OnModelCreating(modelBuilder);
    }*/
    
    public List<Movie> Movies { get; set; } = new List<Movie>
    {
        new Movie
        {
            Id = 1, Title = "The Shawshank Redemption", Writer = "Stephen King", Director = "Frank Darabont", MPA = "R",
            Genre = "Drama", Rating = 9.3
        },
        new Movie
        {
            Id = 2, Title = "The Godfather", Writer = "Mario Puzo", Director = "Francis Ford Coppola", MPA = "R",
            Genre = "Crime", Rating = 9.2
        },
        new Movie
        {
            Id = 3, Title = "Pulp Fiction", Writer = "Quentin Tarantino", Director = "Quentin Tarantino", MPA = "R",
            Genre = "Crime", Rating = 8.9
        },
        new Movie
        {
            Id = 4, Title = "The Dark Knight", Writer = "Jonathan Nolan", Director = "Christopher Nolan", MPA = "PG-13",
            Genre = "Action", Rating = 9.0
        },
        new Movie
        {
            Id = 5, Title = "Fight Club", Writer = "Chuck Palahniuk", Director = "David Fincher", MPA = "R",
            Genre = "Drama", Rating = 8.8
        },
        new Movie
        {
            Id = 6, Title = "Inception", Writer = "Christopher Nolan", Director = "Christopher Nolan", MPA = "PG-13",
            Genre = "Sci-Fi", Rating = 8.7
        },
        new Movie
        {
            Id = 7, Title = "The Matrix", Writer = "Lana and Lilly Wachowski", Director = "Lana and Lilly Wachowski",
            MPA = "R", Genre = "Sci-Fi", Rating = 8.7
        },
        new Movie
        {
            Id = 8, Title = "Interstellar", Writer = "Jonathan Nolan", Director = "Christopher Nolan", MPA = "PG-13",
            Genre = "Sci-Fi", Rating = 8.6
        },
        new Movie
        {
            Id = 9, Title = "Parasite", Writer = "Bong Joon-ho", Director = "Bong Joon-ho", MPA = "R",
            Genre = "Thriller", Rating = 8.5
        },
        new Movie
        {
            Id = 10, Title = "Whiplash", Writer = "Damien Chazelle", Director = "Damien Chazelle", MPA = "R",
            Genre = "Drama", Rating = 8.5
        },
        new Movie
        {
            Id = 11, Title = "Goodfellas", Writer = "Nicholas Pileggi", Director = "Martin Scorsese", MPA = "R",
            Genre = "Crime", Rating = 8.7
        },
        new Movie
        {
            Id = 12, Title = "The Silence of the Lambs", Writer = "Thomas Harris", Director = "Jonathan Demme",
            MPA = "R", Genre = "Thriller", Rating = 8.6
        },
        new Movie
        {
            Id = 13, Title = "Schindler's List", Writer = "Thomas Keneally", Director = "Steven Spielberg", MPA = "R",
            Genre = "Biography", Rating = 8.9
        },
        new Movie
        {
            Id = 14, Title = "Forrest Gump", Writer = "Winston Groom", Director = "Robert Zemeckis", MPA = "PG-13",
            Genre = "Drama", Rating = 8.8
        },
        new Movie
        {
            Id = 15, Title = "The Lord of the Rings: The Fellowship of the Ring", Writer = "J.R.R. Tolkien",
            Director = "Peter Jackson", MPA = "PG-13", Genre = "Fantasy", Rating = 8.8
        },
        new Movie
        {
            Id = 16, Title = "The Lord of the Rings: The Two Towers", Writer = "J.R.R. Tolkien",
            Director = "Peter Jackson", MPA = "PG-13", Genre = "Fantasy", Rating = 8.7
        },
        new Movie
        {
            Id = 17, Title = "The Lord of the Rings: The Return of the King", Writer = "J.R.R. Tolkien",
            Director = "Peter Jackson", MPA = "PG-13", Genre = "Fantasy", Rating = 8.9
        },
        new Movie
        {
            Id = 18, Title = "Saving Private Ryan", Writer = "Robert Rodat", Director = "Steven Spielberg", MPA = "R",
            Genre = "War", Rating = 8.6
        },
        new Movie
        {
            Id = 19, Title = "The Green Mile", Writer = "Stephen King", Director = "Frank Darabont", MPA = "R",
            Genre = "Drama", Rating = 8.6
        },
        new Movie
        {
            Id = 20, Title = "Gladiator", Writer = "David Franzoni", Director = "Ridley Scott", MPA = "R",
            Genre = "Action", Rating = 8.5
        },
        new Movie
        {
            Id = 21, Title = "The Departed", Writer = "William Monahan", Director = "Martin Scorsese", MPA = "R",
            Genre = "Crime", Rating = 8.5
        },
        new Movie
        {
            Id = 22, Title = "The Usual Suspects", Writer = "Christopher McQuarrie", Director = "Bryan Singer",
            MPA = "R", Genre = "Crime", Rating = 8.5
        },
        new Movie
        {
            Id = 23, Title = "The Prestige", Writer = "Christopher Priest", Director = "Christopher Nolan",
            MPA = "PG-13", Genre = "Drama", Rating = 8.5
        },
        new Movie
        {
            Id = 24, Title = "Casablanca", Writer = "Murray Burnett", Director = "Michael Curtiz", MPA = "PG",
            Genre = "Romance", Rating = 8.5
        },
        new Movie
        {
            Id = 25, Title = "Rear Window", Writer = "Cornell Woolrich", Director = "Alfred Hitchcock", MPA = "PG",
            Genre = "Mystery", Rating = 8.5
        },
        new Movie
        {
            Id = 26, Title = "Alien", Writer = "Dan O'Bannon", Director = "Ridley Scott", MPA = "R", Genre = "Sci-Fi",
            Rating = 8.4
        },
        new Movie
        {
            Id = 27, Title = "Apocalypse Now", Writer = "John Milius", Director = "Francis Ford Coppola", MPA = "R",
            Genre = "War", Rating = 8.4
        },
        new Movie
        {
            Id = 28, Title = "The Shining", Writer = "Stephen King", Director = "Stanley Kubrick", MPA = "R",
            Genre = "Horror", Rating = 8.4
        },
        new Movie
        {
            Id = 29, Title = "Jurassic Park", Writer = "Michael Crichton", Director = "Steven Spielberg", MPA = "PG-13",
            Genre = "Adventure", Rating = 8.2
        },
        new Movie
        {
            Id = 30, Title = "Blade Runner", Writer = "Philip K. Dick", Director = "Ridley Scott", MPA = "R",
            Genre = "Sci-Fi", Rating = 8.1
        },
        new Movie
        {
            Id = 31, Title = "No Country for Old Men", Writer = "Cormac McCarthy", Director = "Ethan and Joel Coen",
            MPA = "R", Genre = "Crime", Rating = 8.1
        },
        new Movie
        {
            Id = 32, Title = "Eternal Sunshine of the Spotless Mind", Writer = "Charlie Kaufman",
            Director = "Michel Gondry", MPA = "R", Genre = "Drama", Rating = 8.3
        },
        new Movie
        {
            Id = 33, Title = "Memento", Writer = "Christopher Nolan", Director = "Christopher Nolan", MPA = "R",
            Genre = "Mystery", Rating = 8.4
        },
        new Movie
        {
            Id = 34, Title = "The Truman Show", Writer = "Andrew Niccol", Director = "Peter Weir", MPA = "PG",
            Genre = "Drama", Rating = 8.1
        },
        new Movie
        {
            Id = 35, Title = "The Grand Budapest Hotel", Writer = "Stefan Zweig", Director = "Wes Anderson", MPA = "R",
            Genre = "Comedy", Rating = 8.1
        }
    };
}
