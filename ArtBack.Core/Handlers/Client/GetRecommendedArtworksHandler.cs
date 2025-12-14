using ArtBack.Core.Queries.Client;
using ArtBack.Domain.Dtos;
using ArtBack.Domain.Types;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace ArtBack.Core.Handlers.Client;

public class GetRecommendedArtworksHandler
    : IRequestHandler<GetRecommendedArtworksQuery, List<ArtworkDto>>
{
    private readonly ArtDbContext _dbContext;
    private readonly ILogger<GetRecommendedArtworksHandler> _logger;

    public GetRecommendedArtworksHandler(ArtDbContext dbContext, ILogger<GetRecommendedArtworksHandler> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }
    
    public async Task<List<ArtworkDto>> Handle(GetRecommendedArtworksQuery request,
        CancellationToken cancellationToken)
    {
        //liked
        var likedIds = await _dbContext.LikedArtworks
            .Where(x => x.ClientId == request.ClientId && !x.isDeleted)
            .Select(x => x.ArtworkId)
            .ToListAsync(cancellationToken);
        
        //cart
        var cartIds = await _dbContext.CartArtworks
            .Where(x => x.Cart.ClientId == request.ClientId)
            .Select(x => x.ArtworkId)
            .ToListAsync(cancellationToken);
        
        //order
        var orderedIds = await _dbContext.OrderArtworks
            .Where(x => x.Order.ClientId == request.ClientId)
            .Select(x => x.ArtworkId)
            .ToListAsync(cancellationToken);
        
        //unikalius traukia
        var interactedIds = likedIds.Concat(cartIds).Concat(orderedIds).Distinct().ToList();

        
        // jei nera tuscias
        if (!interactedIds.Any())
            return new List<ArtworkDto>();
        
        var interactedArtworks = await _dbContext.Artworks
            .Where(a => interactedIds.Contains(a.Id))
            .Include(a => a.Category)
            .ToListAsync(cancellationToken);
        
        var categoryCounts = new List<(string category, object value, int count)>();
        
        // Style
        categoryCounts.AddRange(interactedArtworks
            .GroupBy(a => a.Category.Style)
            .Select(g => ("Style", (object)g.Key, g.Count())));

        // Author
        categoryCounts.AddRange(interactedArtworks
            .GroupBy(a => a.Author)
            .Select(g => ("Author", (object)g.Key, g.Count())));

        // Technique
        categoryCounts.AddRange(interactedArtworks
            .GroupBy(a => a.Category.Technique)
            .Select(g => ("Technique", (object)g.Key, g.Count())));

        // ColorPalette
        categoryCounts.AddRange(interactedArtworks
            .GroupBy(a => a.Category.ColorPalette)
            .Select(g => ("ColorPalette", (object)g.Key, g.Count())));

        //ArtType
        categoryCounts.AddRange(interactedArtworks
            .GroupBy(a => a.Category.ArtType)
            .Select(g => ("ArtType", (object)g.Key, g.Count())));

        // Period
        categoryCounts.AddRange(interactedArtworks
            .GroupBy(a => a.Category.Period)
            .Select(g => ("Period", (object)g.Key, g.Count())));

        // top 3 categories
        var topCategories = categoryCounts
            .OrderByDescending(c => c.count)
            .Take(3)
            .ToList();
        
        _logger.LogInformation("Top categories for client {ClientId}: {TopCategories}", 
            request.ClientId, 
            string.Join(", ", topCategories.Select(tc => $"{tc.category}:{tc.value}")));

        // exclude interacted
        var candidateArtworks = await _dbContext.Artworks
            .Where(a => !interactedIds.Contains(a.Id))
            .Include(a => a.Category)
            .ToListAsync(cancellationToken);

        var recommended = candidateArtworks
            .Where(a => topCategories.Any(tc =>
                (tc.category == "Style" && a.Category.Style == (Style)tc.value) ||
                (tc.category == "Author" && a.Author == (string)tc.value) ||
                (tc.category == "Technique" && a.Category.Technique == (Technique)tc.value) ||
                (tc.category == "ColorPalette" && a.Category.ColorPalette == (ColorPallete)tc.value) ||
                (tc.category == "ArtType" && a.Category.ArtType == (ArtType)tc.value) ||
                (tc.category == "Period" && a.Category.Period == (Period)tc.value)
            ))
            .Take(10)
            .Select(a => new ArtworkDto
            {
                Id = a.Id,
                Name = a.Name,
                Author = a.Author,
                Description = a.Description,
                CreatedAt = a.CreatedAt,
                Price = a.Price,
                Dimensions = a.Dimensions,
                ImageUrl = a.ImageUrl,
                Style = a.Category.Style,
                Material = a.Category.Material,
                Technique = a.Category.Technique,
                ColorPalette = a.Category.ColorPalette,
                ArtType = a.Category.ArtType,
                Period = a.Category.Period
            })
            .ToList();

        return recommended;
    }
}