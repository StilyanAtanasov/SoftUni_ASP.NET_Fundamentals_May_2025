using BookVerse.ViewModels.Genre;

namespace BookVerse.Services.Core.Contracts;

public interface IGenreService
{
    /// <summary>
    /// Asynchronously retrieves a read-only collection of all genres.
    /// </summary>
    /// <remarks>The returned collection is intended for read-only purposes and should not be modified.  This
    /// method is typically used to fetch genre data for display or selection in user interfaces.</remarks>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of  <see
    /// cref="GenreViewModel"/> objects representing all available genres. The collection is read-only.</returns>
    Task<ICollection<GenreViewModel>> GetAllGenresReadonlyAsync();
}