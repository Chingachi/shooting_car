using Zenject;

namespace Installers
{
    public class DataInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            Container.Bind<PlayerData>().AsSingle();
        }
    }
}