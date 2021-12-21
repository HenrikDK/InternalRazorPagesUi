namespace InternalRazorPagesUi.ReverseProxy;

public class ProxyPath
{
    public string RequestPath { get; set; }
    public string RequestServicePath { get; set; }
    public string ControllerPrefixPath { get; set; }
    public string ServicePath { get; set; }
    public string ServiceAddress { get; set; }
    public Uri? AbsoluteRequestUri { get; set; }
}