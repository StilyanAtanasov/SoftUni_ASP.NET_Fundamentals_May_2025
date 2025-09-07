using BookVerse.Services.Core.Utils;
using BookVerse.ViewModels.Book;

namespace BookVerse.Services.Core.Contracts;

public interface IBookService
{
    /// <summary>
    /// Retrieves a read-only collection of books available to the specified user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user for whom the books are being retrieved.  If <see langword="null"/>, retrieves
    /// books available to all users.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a collection of  <see
    /// cref="BookCardViewModel"/> objects representing the books. The collection will be empty if no books are
    /// available.</returns>
    Task<ICollection<BookCardViewModel>> GetAllBooksReadonlyAsync(string? userId);

    /// <summary>
    /// Retrieves the details of a book in a read-only format.
    /// </summary>
    /// <remarks>This method is intended for read-only operations and does not modify the state of the book or
    /// any related data. Ensure that <paramref name="bookId"/> is valid and corresponds to an existing  book in the
    /// system.</remarks>
    /// <param name="bookId">The unique identifier of the book to retrieve.</param>
    /// <param name="userId">The optional identifier of the user making the request. If provided, the response may include  user-specific
    /// details, such as personalized recommendations or access permissions.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a  <see cref="ServiceResult{T}"/>
    /// object wrapping a <see cref="BookDetailsViewModel"/>  with the book's details. If the book is not found, the
    /// result may indicate an error.</returns>
    Task<ServiceResult<BookDetailsViewModel>> GetBookDetailsReadonlyAsync(int bookId, string? userId);

    /// <summary>
    /// Retrieves a read-only view model for editing a book, tailored to the specified user.
    /// </summary>
    /// <remarks>The returned view model is read-only and intended for display or non-modifiable purposes. 
    /// Ensure the caller has appropriate permissions before invoking this method.</remarks>
    /// <param name="bookId">The unique identifier of the book to retrieve.</param>
    /// <param name="userId">The identifier of the user requesting the book. Can be <see langword="null"/> if user-specific data is not
    /// required.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result contains a <see
    /// cref="ServiceResult{T}"/> with a <see cref="BookFormViewModel"/> for the specified book, or an error if the
    /// operation fails.</returns>
    Task<ServiceResult<BookFormViewModel>> GetBookForEditReadonlyAsync(int bookId, string userId);

    /// <summary>
    /// Retrieves a read-only view model containing details of a book for deletion purposes.
    /// </summary>
    /// <remarks>This method is intended for scenarios where a user needs to confirm the deletion of a book.
    /// The returned view model is read-only and does not allow modifications.</remarks>
    /// <param name="bookId">The unique identifier of the book to retrieve.</param>
    /// <param name="userId">The identifier of the user requesting the operation. Can be <see langword="null"/> if the operation does not
    /// require user context.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a <see cref="ServiceResult{T}"/>
    /// wrapping a <see cref="DeleteBookViewModel"/> with the book details, or an error if the operation fails.</returns>
    Task<ServiceResult<DeleteBookViewModel>> GetBookForDeletionReadonlyAsync(int bookId, string userId);

    /// <summary>
    /// Creates a new book record based on the provided model and associates it with the specified user.
    /// </summary>
    /// <remarks>This method validates the input model and user ID before creating the book.  Ensure that the
    /// <paramref name="model"/> contains valid data and that the <paramref name="userId"/>  corresponds to an existing
    /// user in the system.</remarks>
    /// <param name="model">The data model containing the details of the book to be created. Cannot be null.</param>
    /// <param name="userId">The unique identifier of the user creating the book. Cannot be null or empty.</param>
    /// <returns>A <see cref="ServiceResult{T}"/> indicating the success or failure of the operation.</returns>
    Task<ServiceResult> CreateBookAsync(BookFormViewModel model, string userId);

    /// <summary>
    /// Updates the details of an existing book based on the provided model.
    /// </summary>
    /// <remarks>This method validates the provided model and updates the corresponding book record in the
    /// system. Ensure that the user has the necessary permissions to perform this operation if <paramref
    /// name="userId"/> is provided.</remarks>
    /// <param name="model">The view model containing the updated book details. Must not be null.</param>
    /// <param name="userId">The ID of the user performing the update.</param>
    /// <returns>A <see cref="ServiceResult"/> indicating the success or failure of the operation.  The result contains
    /// additional information about the outcome.</returns>
    Task<ServiceResult> EditBookAsync(BookFormViewModel model, string userId);

    /// <summary>
    /// Marks a book as deleted without permanently removing it from the database.
    /// </summary>
    /// <remarks>This method performs a soft delete by updating the book's status to indicate it is deleted,
    /// rather than removing it entirely from the database. Use this method when you want to retain the book's data for
    /// auditing or recovery purposes.</remarks>
    /// <param name="bookId">The id of the book.</param>
    /// <param name="userId">The user that has started this operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous operation. The result contains a <see
    /// cref="ServiceResult"/> indicating the success or failure of the operation.</returns>
    Task<ServiceResult> SoftDeleteBookAsync(int bookId, string userId);

    Task<ICollection<MyBooksCardViewModel>> GetMyBooksReadonlyAsync(string userId);

    /// <summary>
    /// Adds the specified book to the user's list of favourites asynchronously.
    /// </summary>
    /// <remarks>This method performs the operation asynchronously and may involve database or external
    /// service interactions. Ensure that the provided <paramref name="bookId"/> and <paramref name="userId"/> are valid
    /// before calling this method.</remarks>
    /// <param name="bookId">The unique identifier of the book to add to the favourites list. Must be a valid book ID.</param>
    /// <param name="userId">The unique identifier of the user for whom the book is being added to favourites. Cannot be null or empty.</param>
    /// <returns>A <see cref="ServiceResult"/> indicating the success or failure of the operation.  The result contains
    /// additional details about the operation's outcome.</returns>
    Task<ServiceResult> AddToMyBooksAsync(int bookId, string userId);

    /// <summary>
    /// Removes a book from the user's collection of saved books.
    /// </summary>
    /// <remarks>This method performs an asynchronous operation to remove the specified book from the user's
    /// collection.  Ensure that the provided <paramref name="bookId"/> and <paramref name="userId"/> are valid before
    /// calling this method.</remarks>
    /// <param name="bookId">The unique identifier of the book to be removed. Must be a valid book ID.</param>
    /// <param name="userId">The unique identifier of the user whose collection is being modified. Cannot be null or empty.</param>
    /// <returns>A <see cref="ServiceResult"/> indicating the success or failure of the operation.  The result contains details
    /// about the operation's outcome.</returns>
    Task<ServiceResult> RemoveFromMyBooksAsync(int bookId, string userId);
}