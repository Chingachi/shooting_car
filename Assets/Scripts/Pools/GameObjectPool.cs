using System.Collections.Generic;
using Pools.Interfaces;
using Resolver;
using UnityEngine;

namespace Pools
{
    public class GameObjectPool<T> : IPool<T> where T : Poolable
    {
        protected readonly ContainerResolver _resolver;
        protected readonly GameObject _container;
        protected readonly T _prefabInstance;

        protected Stack<T> _stack = new Stack<T>();

        protected GameObjectPool(ContainerResolver resolver, Poolable[] poolables)
        {
            _resolver = resolver;

            foreach(Poolable poolable in poolables)
            {
                if(poolable is T targetPoolable)
                {
                    _prefabInstance = targetPoolable;
                    break;
                }
            }

            _container = new GameObject($"{GetType().Name} {typeof(T).Name}")
            {
                transform =
                {
                    position = new Vector3(0, 100, 0)
                }
            };
        }

        public virtual void Initialize()
        {

        }

        public virtual T Get()
        {
            if(_stack.Count == 0)
            {
                T newInstance = Instantiate();
                newInstance.FromPool();
                return newInstance;
            }

            T poolable = _stack.Pop();
            poolable.FromPool();
            return poolable;
        }

        public virtual void Return(T poolable)
        {
            poolable.ToPool();
            _stack.Push(poolable);
        }

        protected T Instantiate()
        {
            if(_prefabInstance == null)
            {
                Debug.LogError($"Pool prefab of type [{typeof(T)}] Not Found!");
                return default;
            }

            GameObject go = _resolver.Container.InstantiatePrefab(_prefabInstance, _container.transform);
            go.name = go.name.Replace("(Clone)", "");
            T poolable = go.GetComponent<T>();
            poolable.FromPool();
            return poolable;
        }
    }
}