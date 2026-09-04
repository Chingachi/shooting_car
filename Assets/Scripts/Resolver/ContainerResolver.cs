using Zenject;

namespace Resolver
{
    public class ContainerResolver
    {
        public void SetContainer(DiContainer container)
        {
            Container = container;
        }

        public DiContainer Container
        {
            get;
            private set;
        }
    }
}