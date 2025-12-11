using Microsoft.EntityFrameworkCore;
using ArtBack.Domain.Entities;

namespace ArtBack.Infrastructure;

public class ArtDbContext(DbContextOptions<ArtDbContext> options) : DbContext(options)
{
    public DbSet<Artwork> Artworks { get; set; }
    public DbSet<Category> Categories { get; set; }
}