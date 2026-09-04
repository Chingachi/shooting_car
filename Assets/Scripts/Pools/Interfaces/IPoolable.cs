namespace Pools.Interfaces
{
    public interface IPoolable
    {
        public void ToPool();

        public void FromPool();
    }
}