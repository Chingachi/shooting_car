using Zenject;

namespace Pools.Interfaces
{
    public interface IPool<T> : IInitializable where T : IPoolable
    {
        public T Get();

        public void Return(T poolable);
    }
}