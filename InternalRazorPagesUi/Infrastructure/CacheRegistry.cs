using InternalRazorPagesUi.Model.Cache;

namespace InternalRazorPagesUi.Infrastructure;

public class CacheRegistry : ServiceRegistry
{
    public CacheRegistry()
    {
        For<IGetServicesCached>().Use<GetServicesCached>().Singleton();
    }
}