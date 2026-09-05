using Gameplay.Enemies;
using Gameplay.Shooting;
using Pools.Interfaces;
using UnityEngine;
using Zenject;

namespace Pools.Installer
{
    public class PoolsInstaller : MonoInstaller
    {
        [SerializeField]
        private Poolable[] _poolables;

        public override void InstallBindings()
        {
            Container.Bind<IPool<Projectile>>().To<GameObjectPool<Projectile>>().AsCached().WithArguments(_poolables);
            Container.Bind<IPool<Enemy>>().To<GameObjectPool<Enemy>>().AsCached().WithArguments(_poolables);
        }
    }
}