﻿
using LibraryAPI.Models;
using LibraryAPI.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LibraryAPI.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly LibraryexdbContext _db;
        internal DbSet<T> dbSet;
        public Repository(LibraryexdbContext db)
        {
            _db = db;
            this.dbSet = db.Set<T>();
        }
        public async Task CreateAsync(T entity)
        {
           await dbSet.AddAsync(entity);
            await SaveAsync();
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, int pageSize = 0, int pageNumber = 1)
        {
            IQueryable<T> query = dbSet;
            if (typeof(T) == typeof(Book))
            {
                var books = query.Cast<Book>().ToList();
                foreach (var book in books)
                {
                    if (book.IsBorrowed && book.ReturnDate <= DateTime.Now)
                    {
                        book.IsBorrowed = false; 
                        await SaveAsync();
                    }
                }
                query = query.Cast<Book>().Where(b => !b.IsBorrowed).Cast<T>();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if(pageSize > 0)
            {
                if(pageSize > 100)
                {
                    pageSize = 100;
                }
                query = query.Skip(pageSize * (pageNumber - 1)).Take(pageSize);
            }
            if (includeProperties != null)
            {
                foreach (var includeProp in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProp);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetAsync(Expression<Func<T, bool>>? filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked)
            {
                query = query.AsNoTracking();
            }
            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task RemoveAsync(T entity)
        {
            dbSet.Remove(entity);
            await SaveAsync();
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}
