using BookVerse.Data;
using BookVerse.DataModels;
using BookVerse.Services.Core.Contracts;
using BookVerse.Services.Core.Utils;
using BookVerse.ViewModels.Book;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BookVerse.Services.Core;

public class BookService : IBookService
{
    private readonly ApplicationDbContext _context;
    private readonly UserManager<IdentityUser> _userManager;

    public BookService(ApplicationDbContext context, UserManager<IdentityUser> userManager)
    {
        _context = context;
        _userManager = userManager;
    }

    public async Task<ICollection<BookCardViewModel>> GetAllBooksReadonlyAsync(string? userId) =>
        await _context.Books
            .AsNoTracking()
            .Include(b => b.Genre)
            .Include(b => b.LikedByUsers)
            .Select(b => new BookCardViewModel
            {
                Id = b.Id,
                Title = b.Title,
                CoverImageUrl = b.CoverImageUrl,
                Genre = b.Genre.Name,
                SavedCount = b.LikedByUsers.Count,
                IsAuthor = userId != null && b.PublisherId == userId,
                IsSaved = userId != null && b.LikedByUsers.Any(u => u.Id == userId)
            })
            .ToArrayAsync();

    public async Task<ServiceResult<BookDetailsViewModel>> GetBookDetailsReadonlyAsync(int bookId, string? userId)
    {
        BookDetailsViewModel? bookDetails = await _context.Books
            .AsNoTracking()
            .Include(b => b.Genre)
            .Include(b => b.Publisher)
            .Select(b => new BookDetailsViewModel
            {
                Id = b.Id,
                Title = b.Title,
                Description = b.Description,
                CoverImageUrl = b.CoverImageUrl,
                PublishedOn = b.PublishedOn,
                Publisher = b.Publisher.UserName!,
                IsAuthor = userId != null && b.PublisherId == userId,
                IsSaved = userId != null && b.LikedByUsers.Any(u => u.Id == userId),
                Genre = b.Genre.Name
            })
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (bookDetails == null) return ServiceResult<BookDetailsViewModel>.NotFound();

        return ServiceResult<BookDetailsViewModel>.Ok(bookDetails);
    }

    public async Task<ServiceResult<BookFormViewModel>> GetBookForEditReadonlyAsync(int bookId, string userId)
    {
        Book? book = await _context.Books
            .AsNoTracking()
            .Include(b => b.Genre)
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null) return ServiceResult<BookFormViewModel>.NotFound();
        if (book.PublisherId != userId) return ServiceResult<BookFormViewModel>.Forbidden();

        BookFormViewModel model = new()
        {
            Id = book.Id,
            Title = book.Title,
            Description = book.Description,
            CoverImageUrl = book.CoverImageUrl,
            PublishedOn = book.PublishedOn,
            GenreId = book.GenreId
        };

        return ServiceResult<BookFormViewModel>.Ok(model);
    }

    public async Task<ServiceResult<DeleteBookViewModel>> GetBookForDeletionReadonlyAsync(int bookId, string userId)
    {
        Book? book = await _context.Books
            .AsNoTracking()
            .Include(b => b.Publisher)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null) return ServiceResult<DeleteBookViewModel>.NotFound();
        if (book.PublisherId != userId) return ServiceResult<DeleteBookViewModel>.Forbidden();

        DeleteBookViewModel model = new()
        {
            Id = book.Id,
            Title = book.Title,
            Publisher = book.Publisher.UserName!,
        };

        return ServiceResult<DeleteBookViewModel>.Ok(model);
    }

    public async Task<ServiceResult> CreateBookAsync(BookFormViewModel model, string userId)
    {
        Book book = new()
        {
            Title = model.Title,
            Description = model.Description,
            CoverImageUrl = model.CoverImageUrl,
            PublishedOn = model.PublishedOn,
            GenreId = model.GenreId,
            PublisherId = userId
        };

        await _context.Books.AddAsync(book);
        await _context.SaveChangesAsync();

        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> EditBookAsync(BookFormViewModel model, string userId)
    {
        Book? book = await _context.Books.FirstOrDefaultAsync(b => b.Id == model.Id);
        if (book == null) return ServiceResult.NotFound();
        if (book.PublisherId != userId) return ServiceResult.Forbidden();

        book.Title = model.Title;
        book.Description = model.Description;
        book.CoverImageUrl = model.CoverImageUrl;
        book.PublishedOn = model.PublishedOn;
        book.GenreId = model.GenreId;

        _context.Books.Update(book);
        await _context.SaveChangesAsync();

        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> SoftDeleteBookAsync(int bookId, string userId)
    {
        Book? book = _context.Books.FirstOrDefault(b => b.Id == bookId);

        if (book == null) return ServiceResult.NotFound();
        if (book.PublisherId != userId) return ServiceResult.Forbidden();

        book.IsDeleted = true;
        await _context.SaveChangesAsync();

        return ServiceResult.Ok();
    }

    public async Task<ICollection<MyBooksCardViewModel>> GetMyBooksReadonlyAsync(string userId)
        => await _context.Books
            .AsNoTracking()
            .Where(b => b.LikedByUsers.Any(u => u.Id == userId))
            .Select(b => new MyBooksCardViewModel
            {
                Id = b.Id,
                Title = b.Title,
                CoverImageUrl = b.CoverImageUrl,
                Genre = b.Genre.Name
            })
            .ToArrayAsync();


    public async Task<ServiceResult> AddToMyBooksAsync(int bookId, string userId)
    {
        Book? book = await _context.Books
            .Include(b => b.LikedByUsers)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null) return ServiceResult.NotFound();
        if (book.PublisherId == userId) return ServiceResult.Forbidden();

        if (book.LikedByUsers.All(u => u.Id != userId))
        {
            IdentityUser? user = await _userManager.FindByIdAsync(userId);
            if (user == null) return ServiceResult.NotFound();

            book.LikedByUsers.Add(user);
            await _context.SaveChangesAsync();
        }

        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> RemoveFromMyBooksAsync(int bookId, string userId)
    {
        Book? book = await _context.Books
            .Include(b => b.LikedByUsers)
            .FirstOrDefaultAsync(b => b.Id == bookId);

        if (book == null) return ServiceResult.NotFound();
        if (book.PublisherId == userId) return ServiceResult.Forbidden();

        IdentityUser? user = await _userManager.FindByIdAsync(userId);
        if (user == null) return ServiceResult.NotFound();

        book.LikedByUsers.Remove(user);
        await _context.SaveChangesAsync();

        return ServiceResult.Ok();
    }
}