using System;
using System.Collections.Generic;
using System.Text;
using Movies.Shared;
using Movies.Shared.Interfaces;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Movies.Infrastructure.Data
{
    public class EfRepository : IRepository
    {
        private readonly MoviesDbContext _dbContext;

        public EfRepository(MoviesDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public T GetById<T>(int id) where T : BaseEntity
        {
            return _dbContext.Set<T>().AsNoTracking().SingleOrDefault(e => e. Id == id);
        }

        public List<T> List<T>() where T : BaseEntity
        {
            return _dbContext.Set<T>().AsNoTracking().ToList();
        }

        public T Add<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
            
            return entity;
        }

        public void Delete<T>(T entity) where T : BaseEntity
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Update<T>(T entity) where T : BaseEntity
        {
            _dbContext.Entry(entity).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }
    }
}
