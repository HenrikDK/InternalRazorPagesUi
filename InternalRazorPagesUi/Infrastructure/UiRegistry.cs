using InternalRazorPagesUi.Model.Cache;
using Lamar;

namespace InternalRazorPagesUi.Infrastructure;

public class UiRegistry : ServiceRegistry
{
    public UiRegistry()
    {
        Scan(x =>
        {
            x.AssemblyContainingType<Program>();
                
            x.WithDefaultConventions();

            x.LookForRegistries();
                
            x.ExcludeType<UiRegistry>();
        });

        For<IGetServicesCached>().Use<GetServicesCached>().Singleton();
    }
}