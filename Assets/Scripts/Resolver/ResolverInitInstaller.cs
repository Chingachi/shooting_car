using Zenject;

namespace Resolver
{
    public class ResolverInitInstaller : MonoInstaller
    {
        public override void InstallBindings()
        {
            ContainerResolver resolver = new ContainerResolver();
            resolver.SetContainer(Container);
            Container.Bind<ContainerResolver>().FromInstance(resolver).AsSingle().NonLazy();
        }
    }
}