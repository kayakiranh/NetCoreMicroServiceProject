using System;

namespace MP.Core.Application.Repositories
{
    public interface ICacheRepository
    {
        T GetData<T>(string key);
        void SetData<T>(string key, T value);
        void RemoveData(string key);
    }
}
