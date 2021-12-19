using InternalRazorPagesUi.Model.Queries;
using Microsoft.Extensions.Caching.Memory;

namespace InternalRazorPagesUi.Model.Cache;

public interface IGetServicesCached
{
    IDictionary<string, string> Execute();
}
    
public class GetServicesCached : IGetServicesCached
{
    private readonly IMemoryCache _cache;
    private readonly IGetServices _getServices;

    public GetServicesCached(IMemoryCache cache, IGetServices getServices)
    {
        _cache = cache;
        _getServices = getServices;
    }

    public IDictionary<string, string> Execute()
    {
        return _cache.GetOrCreate("services", x =>
        {
            x.AbsoluteExpiration = DateTime.Now.AddMinutes(15);
            return _getServices.Execute();
        });
    }
}