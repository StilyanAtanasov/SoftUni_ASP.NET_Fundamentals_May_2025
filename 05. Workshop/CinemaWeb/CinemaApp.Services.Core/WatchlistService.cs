using CinemaApp.Data;
using CinemaApp.Data.Models;
using CinemaApp.Services.Core.Interfaces;
using CinemaApp.Web.ViewModels.Watchlist;
using Microsoft.EntityFrameworkCore;
using static CinemaApp.Data.Common.EntityConstraints.Movie;

namespace CinemaApp.Services.Core;

public class WatchlistService : IWatchlistService
{
    private readonly CinemaAppDbContext _context;

    public WatchlistService(CinemaAppDbContext context) => _context = context;

    public async Task<IEnumerable<WatchlistViewModel>> GetUserWatchlistAsync(string userId) =>
        await _context.UsersMovies
            .AsNoTracking()
            .Where(um => um.UserId == userId)
            .Select(um => new WatchlistViewModel
            {
                MovieId = um.MovieId.ToString(),
                Title = um.Movie.Title,
                Genre = um.Movie.Genre,
                ReleaseDate = um.Movie.ReleaseDate.ToString(ReleaseDateFormat),
                ImageUrl = um.Movie.ImageUrl,
            })
            .ToArrayAsync();

    public async Task<bool> IsMovieInWatchlistAsync(string userId, Guid movieId) =>
        await _context.UsersMovies
            .AnyAsync(um => um.UserId == userId && um.MovieId == movieId);

    public async Task AddToWatchlistAsync(string userId, string movieId)
    {
        UserMovie userMovie = new()
        {
            UserId = userId,
            MovieId = Guid.Parse(movieId)
        };

        await _context.UsersMovies.AddAsync(userMovie);
        await _context.SaveChangesAsync();
    }

    public async Task RemoveFromWatchlistAsync(string userId, string movieId)
    {
        UserMovie? userMovie = await _context.UsersMovies
            .FirstOrDefaultAsync(um => um.MovieId == Guid.Parse(movieId) && um.UserId == userId);

        if (userMovie != null)
        {
            _context.UsersMovies.Remove(userMovie);
            await _context.SaveChangesAsync();
        }
    }
}