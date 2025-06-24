using CinemaApp.Data;
using CinemaApp.Web.ViewModels.Movie;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaApp.Web.Controllers
{
    public class MovieController : Controller
    {
        private readonly ApplicationDbContext _context;

        public MovieController(ApplicationDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            MovieCardViewModel[] movies = await _context.Movies
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

            return View(movies);
        }
    }
}
