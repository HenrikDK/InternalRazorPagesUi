namespace InternalRazorPagesUi.Model.Repositories;

public interface IServiceRepository
{
    IDictionary<string, string> GetAll();
}
    
public class ServiceRepository : IServiceRepository
{
    private readonly IMemoryCache _cache;

    public ServiceRepository(IMemoryCache cache)
    {
        _cache = cache;
    }
    
    List<(string service, string url)> _services = new()
    {
        ("Orders", "http://localhost:9080"),
        ("Inventory", "http://localhost:9080")
    };
    
    public IDictionary<string, string> GetAll()
    {
        return _cache.GetOrCreate("services", x =>
        {
            x.AbsoluteExpiration = DateTime.Now.AddMinutes(15);
            return GetAllFromService();
        });
    }
    
    private IDictionary<string, string> GetAllFromService()
    {
        return _services.ToDictionary(x => x.service, x => x.url);
    }
}