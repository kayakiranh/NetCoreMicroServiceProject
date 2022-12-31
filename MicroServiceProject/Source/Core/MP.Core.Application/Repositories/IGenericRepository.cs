using MP.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Generic Repository for Entity Framework
    /// </summary>
    /// <typeparam name="T">T object, which class call it</typeparam>
    public interface IGenericRepository<T> where T : BaseEntity
    {
        public Task<List<T>> GetAll();

        public Task<int> GetAllCount();

        public Task<T> GetById(long id);

        public Task<T> Insert(T model);

        public Task<List<T>> MultipleInsert(List<T> model);

        public Task<T> Update(T model);

        public void Remove(int id);
    }
}