﻿using MovieService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieService.Data.MovieData
{
    public class MovieRepo : IMovieRepo
    {
        private readonly MovieContext _context;

        public MovieRepo(MovieContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Movie>> GetAllAsync()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(int id)
        {
            return await _context.Movies.FindAsync(id);
        }

        public async Task<Movie> GetByUrlAsync(string url)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.MovieUrl == url);
        }

        public async Task InsertAsync(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            await _context.Movies.AddAsync(movie);
            await SaveChangesAsync();
        }

        public async Task UpdateAsync(Movie movie)
        {
            if (movie == null)
            {
                throw new ArgumentNullException(nameof(movie));
            }

            _context.Entry(movie).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DisableMovieAsync(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                movie.Status = "Disable"; 
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
