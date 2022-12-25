using System;

namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Cache repository
    /// </summary>
    public interface ICacheRepository
    {
        T GetData<T>(int dbNumber, string key);
        void SetData<T>(int dbNumber, string key, T value);
        void RemoveData(int dbNumber, string key);
    }
}