using ScriptableObjects;
using UnityEngine;
using Zenject;

namespace Installers
{
    public class ConfigsInstaller : MonoInstaller
    {
        [SerializeField]
        private GameConfigSo _gameConfig;

        public override void InstallBindings()
        {
            Container.Bind<GameConfigSo>().FromInstance(_gameConfig).AsSingle();
        }
    }
}