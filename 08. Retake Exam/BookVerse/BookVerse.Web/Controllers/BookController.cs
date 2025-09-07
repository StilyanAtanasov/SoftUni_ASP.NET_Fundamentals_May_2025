using BookVerse.Services.Core.Contracts;
using BookVerse.Services.Core.Utils;
using BookVerse.ViewModels.Book;
using BookVerse.ViewModels.Genre;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookVerse.Web.Controllers;

public class BookController : BaseController
{
    private readonly IBookService _bookService;
    private readonly IGenreService _genreService;

    public BookController(IBookService bookService, IGenreService genreService)
    {
        _bookService = bookService;
        _genreService = genreService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index()
    {
        string? userId = GetUserId();
        ICollection<BookCardViewModel> books = await _bookService.GetAllBooksReadonlyAsync(userId);

        return View(books);
    }

    [HttpGet("Book/Details/{bookId}")]
    [AllowAnonymous]
    public async Task<IActionResult> Details([FromRoute] int bookId)
    {
        string? userId = GetUserId();

        ServiceResult<BookDetailsViewModel> sr = await _bookService.GetBookDetailsReadonlyAsync(bookId, userId);

        if (sr is { Success: false, Found: false }) return NotFound();

        return View(sr.Result);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ICollection<GenreViewModel> genres = await _genreService.GetAllGenresReadonlyAsync();
        BookFormViewModel model = new() { Genres = genres };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Create(BookFormViewModel model)
    {
        string? userId = GetUserId();
        if (userId == null) return Unauthorized();

        if (!ModelState.IsValid)
        {
            model.Genres = await _genreService.GetAllGenresReadonlyAsync();
            return View(model);
        }

        ServiceResult sr = await _bookService.CreateBookAsync(model, userId);
        if (!sr.Success)
        {
            foreach (var error in sr.Errors) ModelState.AddModelError(error.Key, error.Value);
            model.Genres = await _genreService.GetAllGenresReadonlyAsync();

            return View(model);
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Book/Edit/{bookId}")]
    public async Task<IActionResult> Edit(int bookId)
    {
        string? userId = GetUserId();
        if (userId == null) return Unauthorized();

        ServiceResult<BookFormViewModel> sr = await _bookService.GetBookForEditReadonlyAsync(bookId, userId);
        if (!sr.Found) return NotFound();
        if (!sr.HasPermission) return Unauthorized();

        ICollection<GenreViewModel> genres = await _genreService.GetAllGenresReadonlyAsync();

        BookFormViewModel model = sr.Result!;
        model.Genres = genres;

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(BookFormViewModel model)
    {
        string? userId = GetUserId();
        if (userId == null) return Unauthorized();

        if (!ModelState.IsValid)
        {
            model.Genres = await _genreService.GetAllGenresReadonlyAsync();
            return View(model);
        }

        ServiceResult sr = await _bookService.EditBookAsync(model, userId);
        if (!sr.Success)
        {
            foreach (var error in sr.Errors) ModelState.AddModelError(error.Key, error.Value);
            model.Genres = await _genreService.GetAllGenresReadonlyAsync();

            return View(model);
        }

        return RedirectToAction(nameof(Details), new { bookId = model.Id });
    }

    [HttpGet("Book/Delete/{bookId}")]
    public async Task<IActionResult> Delete(int bookId)
    {
        string? userId = GetUserId();
        if (userId == null) return Unauthorized();

        ServiceResult<DeleteBookViewModel> sr = await _bookService.GetBookForDeletionReadonlyAsync(bookId, userId);
        if (!sr.Found) return NotFound();
        if (!sr.HasPermission) return Unauthorized();

        return View(sr.Result);
    }

    [HttpPost("Book/ConfirmDelete/{bookId}")]
    public async Task<IActionResult> ConfirmDelete(int bookId)
    {
        string? userId = GetUserId();
        if (userId == null) return Unauthorized();

        ServiceResult sr = await _bookService.SoftDeleteBookAsync(bookId, userId);
        if (!sr.Found) return NotFound();
        if (!sr.HasPermission) return Unauthorized();

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("Book/MyBooks")]
    public async Task<IActionResult> MyBooks()
    {
        string? userId = GetUserId();
        if (userId == null) return Unauthorized();

        ICollection<MyBooksCardViewModel> model = await _bookService.GetMyBooksReadonlyAsync(userId);

        return View(model);
    }

    [HttpPost("Book/AddToMyBooks/{bookId}")]
    public async Task<IActionResult> AddToMyBooks(int bookId)
    {
        string? userId = GetUserId();
        if (userId == null) return Unauthorized();

        ServiceResult sr = await _bookService.AddToMyBooksAsync(bookId, userId);

        if (!sr.Success)
        {
            if (!sr.Found) return NotFound();
            if (!sr.HasPermission) return Unauthorized();
        }

        string referer = Request.Headers["Referer"].ToString();
        return string.IsNullOrEmpty(referer) ? RedirectToAction(nameof(Index), "Home") : Redirect(referer);
    }

    [HttpPost("Book/RemoveFromMyBooks/{bookId}")]
    public async Task<IActionResult> RemoveFromMyBooks(int bookId)
    {
        string? userId = GetUserId();
        if (userId == null) return Unauthorized();

        ServiceResult sr = await _bookService.RemoveFromMyBooksAsync(bookId, userId);

        if (!sr.Success)
        {
            if (!sr.Found) return NotFound();
            if (!sr.HasPermission) return Unauthorized();
        }

        return RedirectToAction(nameof(MyBooks));
    }
}