using InternalRazorPagesUi.Model.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace InternalRazorPagesUi.Model.Cache;

public interface IGetServicesCached
{
    IDictionary<string, string> Execute();
}
    
public class GetServicesCached : IGetServicesCached
{
    private readonly IMemoryCache _cache;
    private readonly IServiceRepository _serviceRepository;

    public GetServicesCached(IMemoryCache cache, IServiceRepository serviceRepository)
    {
        _cache = cache;
        _serviceRepository = serviceRepository;
    }

    public IDictionary<string, string> Execute()
    {
        return _cache.GetOrCreate("services", x =>
        {
            x.AbsoluteExpiration = DateTime.Now.AddMinutes(15);
            return _serviceRepository.Execute();
        });
    }
}