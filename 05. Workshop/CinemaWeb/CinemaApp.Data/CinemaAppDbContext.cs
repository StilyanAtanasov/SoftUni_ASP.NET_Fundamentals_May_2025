﻿using CinemaApp.Data.Models;
using System.Reflection;

namespace CinemaApp.Data;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
public class CinemaAppDbContext : IdentityDbContext
{
    public CinemaAppDbContext(DbContextOptions<CinemaAppDbContext> options) : base(options) { }

    public virtual DbSet<Movie> Movies { get; set; } = null!;

    public virtual DbSet<UserMovie> UsersMovies { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}