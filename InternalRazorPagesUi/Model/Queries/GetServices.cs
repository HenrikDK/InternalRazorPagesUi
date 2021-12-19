namespace InternalRazorPagesUi.Model.Queries;

public interface IGetServices
{
    IDictionary<string, string> Execute();
}
    
public class GetServices : IGetServices
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