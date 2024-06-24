using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System;

namespace OpenAPI.Identity.Data
{
    public class Repository<TEntity, TId> : IRepository<TEntity,TId> where TEntity : class
    {
        private readonly ApplicationDbContext _context;

        public Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TEntity> CreateAsync(TEntity entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity?> GetByIdAsync(TId id)
        {
            return await _context.Set<TEntity>().FindAsync(id);
        }
    }
}
