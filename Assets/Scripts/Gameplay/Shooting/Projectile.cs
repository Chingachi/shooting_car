using Pools;
using Pools.Interfaces;
using UnityEngine;
using Zenject;
using Vector3 = System.Numerics.Vector3;

namespace Gameplay.Shooting
{
    public class Projectile : Poolable
    {
        [Inject]
        private IPool<Projectile> _pool;

        public void Shoot(Vector3 direction)
        {

        }

        private void OnBecameInvisible()
        {
            if(_pool == null)
            {
                Debug.LogError("Pool for projectile has not been injected!");
                return;
            }

            _pool.Return(this);
        }
    }
}