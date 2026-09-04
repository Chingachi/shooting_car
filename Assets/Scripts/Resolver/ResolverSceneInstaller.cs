using Zenject;

namespace Resolver
{
    public class ResolverSceneInstaller : MonoInstaller
    {
        [Inject]
        private ContainerResolver _resolver;

        public override void InstallBindings()
        {
            _resolver.SetContainer(Container);
        }
    }
}