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

        List<T> GetListByName<T>(string nameSearch);

        T GetByName<T>(string nameSearch);

        List<T> GetListByValue<T>(string valueProperty, string valueSearch);

        T GetByValue<T>(string valueProperty, string valueSearch);
    }
}