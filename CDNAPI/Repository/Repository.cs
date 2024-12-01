using CDNAPI.Interfaces;
using CDNAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CDNAPI.Repository
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : Entity, new()
    {
        protected readonly ApiDbContext _apiDbContext;
        protected readonly DbSet<TEntity> DbSet;

        public Repository(ApiDbContext apiDbContext )
        {
            _apiDbContext = apiDbContext;
            DbSet = _apiDbContext.Set<TEntity>();
        }

        public virtual async Task Add(TEntity entity)
        {
            DbSet.Add(entity);
            await SaveChanges();
        }
        public async Task<TEntity> GetById(Guid Id)
        {
            return await DbSet.FindAsync(Id);
        }

        public void Dispose()
        {
            _apiDbContext?.Dispose();
        }

        public async Task<int> SaveChanges()
        {
            return await _apiDbContext.SaveChangesAsync();
        }
    }
}
