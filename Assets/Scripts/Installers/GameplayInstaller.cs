using UnityEngine;
using Zenject;

namespace Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private Car _car;

        public override void InstallBindings()
        {
            Container.Bind<Car>().FromInstance(_car).AsCached();
        }
    }
}