using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Installers
{
    public class GameplayInstaller : MonoInstaller
    {
        [SerializeField]
        private Car _car;
        [SerializeField]
        private EventSystem _eventSystem;

        public override void InstallBindings()
        {
            Container.Bind<Car>().FromInstance(_car).AsCached();
            Container.Bind<EventSystem>().FromInstance(_eventSystem).AsCached();
        }

    }
}