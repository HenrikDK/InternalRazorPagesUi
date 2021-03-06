using InternalRazorPagesUi.Model.Repositories;

namespace InternalRazorPagesUi.ReverseProxy;

public interface IProxyRequest
{
  public Task<(ProxyPath, HttpResponseMessage)> RequestFromService(HttpContext context, string proxyPrefix = "/sp/");
}

public class ProxyRequest : IProxyRequest
{
  private readonly IServiceRepository _serviceRepository;

  private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler { AllowAutoRedirect = false });
    
  public ProxyRequest(IServiceRepository serviceRepository)
  {
    _serviceRepository = serviceRepository;
  }

  public async Task<(ProxyPath, HttpResponseMessage)> RequestFromService(HttpContext context, string proxyPrefix = "/sp/")
  {
    var proxyRequest = GetProxyRequest(context.Request, proxyPrefix);
    if (proxyRequest.AbsoluteRequestUri == null) return (proxyRequest, new HttpResponseMessage {StatusCode = HttpStatusCode.NotFound});
      
    var targetRequestMessage = CreateTargetMessage(context, proxyRequest.AbsoluteRequestUri);
    var responseMessage = await _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);

    CopyFromTargetResponseHeaders(context, responseMessage);
      
    return (proxyRequest, responseMessage);
  }

  private ProxyPath GetProxyRequest(HttpRequest request, string proxyPrefix)
  {
    var result = new ProxyPath();
      
    var fullUrl = request.GetDisplayUrl();
    var requestUrl = fullUrl[(fullUrl.IndexOf(proxyPrefix) + 4)..];
    result.RequestPath = requestUrl;
      
    var prefix = fullUrl[(fullUrl.IndexOf("://") + 3)..(fullUrl.IndexOf(proxyPrefix) + 4)];
    result.ControllerPrefixPath = prefix[(prefix.IndexOf("/") + 1)..];
      
    var segments = requestUrl.Split("/");
    var serviceRequest = segments.First();
    result.RequestServicePath = Url.Combine("/", result.ControllerPrefixPath, serviceRequest);
      
    var services = _serviceRepository.GetAll();
    if (!services.ContainsKey(serviceRequest)) return result;

    var remainingPath = "";
    if (segments.Length > 1)
    {
      var remaining = segments[1..^0];
      remainingPath = string.Join("/", remaining);
    }

    result.ServicePath = remainingPath;
    result.ServiceAddress = services[serviceRequest];
    var url = Url.Combine(result.ServiceAddress, remainingPath);
    result.AbsoluteRequestUri = new Uri(url);
    return result;
  }
    
  private HttpRequestMessage CreateTargetMessage(HttpContext context, Uri targetUri)
  {
    var requestMessage = new HttpRequestMessage();
    CopyFromOriginalRequestContentAndHeaders(context, requestMessage);

    requestMessage.RequestUri = targetUri;
    requestMessage.Headers.Host = targetUri.Host;
    requestMessage.Method = GetMethod(context.Request.Method);

    return requestMessage;
  }

  private void CopyFromOriginalRequestContentAndHeaders(HttpContext context, HttpRequestMessage requestMessage)
  {
    var requestMethod = context.Request.Method;

    if (!HttpMethods.IsGet(requestMethod) &&
        !HttpMethods.IsHead(requestMethod) &&
        !HttpMethods.IsDelete(requestMethod) &&
        !HttpMethods.IsTrace(requestMethod))
    {
      var streamContent = new StreamContent(context.Request.Body);
      requestMessage.Content = streamContent;
    }

    foreach (var header in context.Request.Headers)
    {
      requestMessage.Content?.Headers.TryAddWithoutValidation(header.Key, header.Value.ToArray());
    }
  }

  private void CopyFromTargetResponseHeaders(HttpContext context, HttpResponseMessage responseMessage)
  {
    foreach (var header in responseMessage.Headers)
    {
      context.Response.Headers[header.Key] = header.Value.ToArray();
    }

    foreach (var header in responseMessage.Content.Headers)
    {
      context.Response.Headers[header.Key] = header.Value.ToArray();
    }
    context.Response.Headers.Remove("transfer-encoding");
  }
  private static HttpMethod GetMethod(string method)
  {
    if (HttpMethods.IsDelete(method)) return HttpMethod.Delete;
    if (HttpMethods.IsGet(method)) return HttpMethod.Get;
    if (HttpMethods.IsHead(method)) return HttpMethod.Head;
    if (HttpMethods.IsOptions(method)) return HttpMethod.Options;
    if (HttpMethods.IsPost(method)) return HttpMethod.Post;
    if (HttpMethods.IsPut(method)) return HttpMethod.Put;
    if (HttpMethods.IsTrace(method)) return HttpMethod.Trace;
    return new HttpMethod(method);
  }
}