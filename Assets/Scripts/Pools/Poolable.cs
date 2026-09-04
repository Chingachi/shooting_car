using Pools.Interfaces;
using UnityEngine;

namespace Pools
{
    public abstract class Poolable : MonoBehaviour, IPoolable
    {
        public virtual void ToPool()
        {
            gameObject.SetActive(false);
        }

        public virtual void FromPool()
        {
            gameObject.SetActive(true);
        }
    }
}