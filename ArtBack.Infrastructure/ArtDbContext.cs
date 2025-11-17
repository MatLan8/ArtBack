using Microsoft.EntityFrameworkCore;

namespace ArtBack.Infrastructure;

public class ArtDbContext(DbContextOptions<ArtDbContext> options) : DbContext(options)
{
}