using InternalRazorPagesUi.Model.Cache;
using Lamar;

namespace InternalRazorPagesUi.Infrastructure;

public class CacheRegistry : ServiceRegistry
{
    public CacheRegistry()
    {
        For<IGetServicesCached>().Use<GetServicesCached>().Singleton();
    }
}