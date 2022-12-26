using MP.Core.Domain.Entities;
using System.Collections.Generic;

namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Cache repository
    /// </summary>
    public interface ICacheRepository
    {
        T GetData<T>(string key);
        void SetData<T>(string key, T value);
        void RemoveData(BaseEntity baseEntity, string key);
        List<T> GetAll<T>();
    }
}