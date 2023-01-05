using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MP.Core.Application.Repositories;
using MP.Core.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MP.Infrastructure.Persistance.Mssql.Repositories
{
    /// <summary>
    /// Generic Repository Business. Using Entity Framework
    /// </summary>
    /// <typeparam name="T">T object, which class call it</typeparam>
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        private readonly MicroServiceDbContext _dbContext;
        private readonly IConfiguration _configuration;

        public GenericRepository(MicroServiceDbContext dbContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dbContext = dbContext;
        }

        public async Task<List<T>> GetAll()
        {
            return await _dbContext.Set<T>().AsNoTracking().ToListAsync();
        }

        public IQueryable<T> GetAllIQueryable()
        {
            return _dbContext.Set<T>().AsNoTracking().AsQueryable();
        }

        public async Task<int> GetAllCount()
        {
            return await _dbContext.Set<T>().AsNoTracking().CountAsync();
        }

        public async Task<T> GetById(long id)
        {
            if (id < 1) return null;

            return await _dbContext.Set<T>().AsNoTracking().SingleOrDefaultAsync(t => t.Id == id);
        }

        public async Task<T> Insert(T model)
        {
            await _dbContext.AddAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async Task<List<T>> MultipleInsert(List<T> model)
        {
            await _dbContext.AddRangeAsync(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }

        public async void Remove(int id)
        {
            if (id > 0)
            {
                _dbContext.Remove(id);
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<T> Update(T model)
        {
            _dbContext.Update(model);
            await _dbContext.SaveChangesAsync();
            return model;
        }
    }
}