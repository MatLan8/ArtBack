using ArtBack.Core.Commands.Artwork;
using ArtBack.Infrastructure;
using MediatR;

namespace ArtBack.Core.Handlers.Artwork;


public class CreateArtworkCommandHandler(ArtDbContext dbContext) : IRequestHandler<CreateArtworkCommand, bool>
{
    public async Task<bool> Handle(CreateArtworkCommand request, CancellationToken cancellationToken)
    {
        
        var category = new Domain.Entities.Category
        {
            Style = request.Style,
            Material = request.Material,
            Technique = request.Technique,
            ColorPalette = request.ColorPalette,
            ArtType = request.ArtType,
            Period = request.Period
        };
        
        await dbContext.Categories.AddAsync(category, cancellationToken);
        
        var artwork = new Domain.Entities.Artwork
        {
            Name = request.Name,
            Author = request.Author,
            Description = request.Description,
            CreatedAt = DateTime.Now,
            Price = request.Price,
            Dimensions = request.Dimensions,
            ImageUrl = request.ImageUrl,
            CategoryId = category.Id,
            VendorId = request.VendorId
        };

        await dbContext.Artworks.AddAsync(artwork, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}