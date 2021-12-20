namespace InternalRazorPagesUi.Model.Repositories;

public interface IServiceRepository
{
    IDictionary<string, string> Execute();
}
    
public class ServiceRepository : IServiceRepository
{ 
    List<(string service, string url)> _services = new()
    {
        ("Orders", "http://localhost:8080"),
        ("Inventory", "http://localhost:9090")
    };
    
    public IDictionary<string, string> Execute()
    {
        return _services.ToDictionary(x => x.service, x => x.url);
    }
}