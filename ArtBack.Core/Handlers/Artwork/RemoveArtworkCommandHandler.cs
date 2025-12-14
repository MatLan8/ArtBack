using ArtBack.Core.Commands.Artwork;
using ArtBack.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace ArtBack.Core.Handlers.Artwork;

public class RemoveArtworkCommandHandler(ArtDbContext dbContext) : IRequestHandler<RemoveArtworkCommand, bool>
{
    public async Task<bool> Handle(RemoveArtworkCommand request, CancellationToken cancellationToken)
    {
        using var tx = await dbContext.Database.BeginTransactionAsync(cancellationToken);

        var artwork = await dbContext.Artworks
            .FirstOrDefaultAsync(a => a.Id == request.ArtworkId, cancellationToken)
            ?? throw new Exception($"Artwork with ID {request.ArtworkId} not found");

        artwork.isDeleted = true;

        await dbContext.LikedArtworks
            .Where(la => la.ArtworkId == request.ArtworkId)
            .ExecuteUpdateAsync(
                s => s.SetProperty(la => la.isDeleted, true),
                cancellationToken);

        var cartImpacts = await dbContext.CartArtworks
            .Where(ca => ca.ArtworkId == request.ArtworkId && !ca.isDeleted)
            .GroupBy(ca => ca.CartId)
            .Select(g => new
            {
                CartId = g.Key,
                ArtworkCountToRemove = g.Sum(x => x.ArtworkCount),
                TotalSumToRemove = g.Sum(x => x.ArtworkCount) * artwork.Price
            })
            .ToListAsync(cancellationToken);

        foreach (var impact in cartImpacts)
        {
            await dbContext.Carts
                .Where(c => c.Id == impact.CartId)
                .ExecuteUpdateAsync(
                    s => s
                        .SetProperty(
                            c => c.ArtworkCount,
                            c => Math.Max(0, c.ArtworkCount - impact.ArtworkCountToRemove)
                        )
                        .SetProperty(
                            c => c.TotalSum,
                            c => Math.Max(0, c.TotalSum - impact.TotalSumToRemove)
                        ),
                    cancellationToken);
        }

        await dbContext.CartArtworks
            .Where(ca => ca.ArtworkId == request.ArtworkId)
            .ExecuteUpdateAsync(
                s => s.SetProperty(ca => ca.isDeleted, true),
                cancellationToken);

        var vendor = await dbContext.Vendors
            .FirstOrDefaultAsync(v => v.Id == artwork.VendorId, cancellationToken)
            ?? throw new Exception("Vendor not found");

        vendor.ArtworkCount = Math.Max(0, vendor.ArtworkCount - 1);

        await dbContext.SaveChangesAsync(cancellationToken);
        await tx.CommitAsync(cancellationToken);

        return true;
    }
}