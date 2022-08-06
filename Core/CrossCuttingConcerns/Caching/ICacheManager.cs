namespace Core.CrossCuttingConcerns.Caching
{
    public interface ICacheManager
    {
        void Add(string key, object value, int duration);
        void Remove(string key);
        void RemoveByPattern(string pattern);
        bool IsAdd(string key);
        object Get(string key);
        T Get<T>(string key);

    }
}
