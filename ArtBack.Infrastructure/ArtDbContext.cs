using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using ArtBack.Domain.Entities;

namespace ArtBack.Infrastructure;

public class ArtDbContext(DbContextOptions<ArtDbContext> options) : DbContext(options)
{
    public DbSet<Artwork> Artworks { get; set; }
    public DbSet<Category> Categories { get; set; }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Client> Clients { get; set; }
    public DbSet<Vendor> Vendors { get; set; }
    
    public DbSet<Cart> Carts { get; set; }
    public DbSet<CartArtwork> CartArtworks { get; set; }
    
    public DbSet<LikedArtwork> LikedArtworks { get; set; }
    
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderArtwork> OrderArtworks { get; set; }
    
    public DbSet<DiscountCoupon> DiscountCoupons { get; set; }
    
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<User>()
            .ToTable("Users");

        modelBuilder.Entity<Client>()
            .ToTable("Clients");
        
        modelBuilder.Entity<Vendor>()
            .ToTable("Vendors");

    }
}