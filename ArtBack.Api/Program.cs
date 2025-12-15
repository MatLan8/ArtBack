
using ArtBack.Core.Commands.Artwork;
using ArtBack.Core.Handlers.Client;
using ArtBack.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Stripe;
using MediatR;
using ArtBack.Core;
using ArtBack.Core.Queries.Client;

var builder = WebApplication.CreateBuilder(args);

StripeConfiguration.ApiKey =
    builder.Configuration["Stripe:SecretKey"];

builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateArtworkCommand).Assembly));
builder.Services.AddControllers();

builder.Services.AddDbContext<ArtDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});


builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(ArtBack.Core.Commands.Client.AddLikedArtworkCommand).Assembly);
});


builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173",
            "http://localhost:5174",
                "http://localhost:5176"
                )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "ArtBack")
    );
    
}

app.UseCors();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();