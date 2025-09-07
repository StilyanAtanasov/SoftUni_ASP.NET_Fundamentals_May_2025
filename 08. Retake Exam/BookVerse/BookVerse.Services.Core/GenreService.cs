using BookVerse.Data;
using BookVerse.Services.Core.Contracts;
using BookVerse.ViewModels.Genre;
using Microsoft.EntityFrameworkCore;

namespace BookVerse.Services.Core;

public class GenreService : IGenreService
{
    private readonly ApplicationDbContext _context;

    public GenreService(ApplicationDbContext context) => _context = context;

    public async Task<ICollection<GenreViewModel>> GetAllGenresReadonlyAsync()
        => await _context.Genres
        .AsNoTracking()
        .Select(g => new GenreViewModel
        {
            Id = g.Id,
            Name = g.Name
        })
        .ToArrayAsync();
}