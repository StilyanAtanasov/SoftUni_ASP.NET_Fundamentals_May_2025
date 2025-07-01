using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.IO;
using static CinemaApp.Data.Common.EntityConstraints.Movie;

namespace CinemaApp.Services.Core;

public class MovieService : IMovieService
{
    private readonly CinemaAppDbContext _context;

    public MovieService(CinemaAppDbContext context) => _context = context;

    public async Task<IEnumerable<MovieCardViewModel>> GetAllMoviesAsync() =>
        await _context.Movies
            .AsNoTracking()
            .Where(m => !m.IsDeleted)
            .Select(m => new MovieCardViewModel
            {
                Id = m.Id.ToString(),
                Title = m.Title,
                Director = m.Director,
                Duration = m.Duration.ToString(),
                Genre = m.Genre,
                ImageUrl = m.ImageUrl,
                ReleaseDate = m.ReleaseDate.ToString("yyyy-MM-dd"),
            })
            .ToArrayAsync();

    public async Task AddMovieAsync(MovieFormViewModel model)
    {
        Movie movie = new()
        {
            Title = model.Title,
            Director = model.Director,
            Duration = model.Duration,
            Description = model.Description,
            Genre = model.Genre,
            ImageUrl = model.ImageUrl,
            ReleaseDate = DateTime.ParseExact(model.ReleaseDate, ReleaseDateFormat, CultureInfo.InvariantCulture)
        };

        await _context.Movies.AddAsync(movie);
        await _context.SaveChangesAsync();
    }

    public async Task<MovieDetailsViewModel?> GetByIdAsync(string id)
    {
        Movie? movie = await _context.Movies
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id.ToString() == id && !m.IsDeleted);

        if (movie == null) return null;

        MovieDetailsViewModel viewModel = new()
        {
            Id = movie.Id.ToString(),
            Title = movie.Title,
            Director = movie.Director,
            Duration = movie.Duration,
            Description = movie.Description,
            Genre = movie.Genre,
            ImageUrl = movie.ImageUrl,
            ReleaseDate = movie.ReleaseDate.ToString("yyyy-MM-dd"),
        };

        return viewModel;
    }

    public async Task<MovieFormViewModel?> GetForEditByIdAsync(string id)
    {
        return await _context.Movies
            .Where(m => m.Id.ToString() == id)
            .Select(m => new MovieFormViewModel
            {
                Id = m.Id.ToString(),
                Title = m.Title,
                Genre = m.Genre,
                Director = m.Director,
                Description = m.Description,
                Duration = m.Duration,
                ReleaseDate = m.ReleaseDate.ToString(ReleaseDateFormat),
                ImageUrl = m.ImageUrl
            }).FirstOrDefaultAsync();
    }

    public async Task EditAsync(string id, MovieFormViewModel model)
    {
        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id.ToString() == id);
        if (movie == null) return;

        movie.Title = model.Title;
        movie.Genre = model.Genre;
        movie.Director = model.Director;
        movie.Description = model.Description;
        movie.Duration = model.Duration;
        movie.ReleaseDate = DateTime.ParseExact(model.ReleaseDate, ReleaseDateFormat, CultureInfo.InvariantCulture);
        movie.ImageUrl = model.ImageUrl;

        await _context.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(string id)
    {
        Movie? movie = _context.Movies.FirstOrDefault(m => m.Id.ToString() == id);
        if (movie is { IsDeleted: false })
        {
            movie.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
    }

    public async Task HardDeleteAsync(string id)
    {
        Movie? movie = _context.Movies.FirstOrDefault(m => m.Id.ToString() == id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();
        }
    }
}