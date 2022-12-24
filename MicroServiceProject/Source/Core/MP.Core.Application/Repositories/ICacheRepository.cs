using System;

namespace MP.Core.Application.Repositories
{
    /// <summary>
    /// Cache repository
    /// </summary>
    public interface ICacheRepository
    {
        T GetData<T>(string key);
        void SetData<T>(string key, T value);
        void RemoveData(string key);
    }
}