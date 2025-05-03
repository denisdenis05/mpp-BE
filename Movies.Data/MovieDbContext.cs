//using Movies.Data.Configurations;

using Microsoft.EntityFrameworkCore;
using Movies.Data.Configurations;
using Movies.Data.Models;

namespace Movies.Data;

public class MovieDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public MovieDbContext(DbContextOptions<MovieDbContext> options) : base(options)
    {
    }

    public virtual DbSet<Movie> Movies { get; set; }
    public DbSet<User> Users { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new TestConfiguration());
        base.OnModelCreating(modelBuilder);
    }
}
