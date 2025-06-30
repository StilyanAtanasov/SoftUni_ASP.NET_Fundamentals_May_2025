﻿using CinemaApp.Web.ViewModels.Movie;

namespace CinemaApp.Services.Core.Interfaces;

public interface IMovieService
{
    Task<IEnumerable<MovieCardViewModel>> GetAllMoviesAsync();

    Task AddMovieAsync(MovieFormViewModel model);

    Task<MovieDetailsViewModel?> GetByIdAsync(string id);

    Task<MovieFormViewModel?> GetForEditByIdAsync(string id);

    Task EditAsync(string id, MovieFormViewModel model);

    Task SoftDeleteAsync(string id);

    Task HardDeleteAsync(string id);
}