using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Movies.Data.Models;

namespace Movies.Data.Configurations;

public class EndorsementConfiguration: IEntityTypeConfiguration<Endorsement>
{
    public void Configure(EntityTypeBuilder<Endorsement> builder)
    {
        builder .HasOne(e => e.Movie)
            .WithMany(m => m.Endorsements)
            .HasForeignKey(e => e.MovieId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
