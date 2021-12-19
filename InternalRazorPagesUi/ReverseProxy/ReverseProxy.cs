using System.Net;
using System.Text;
using InternalRazorPagesUi.Model.Cache;

namespace InternalRazorPagesUi.ReverseProxy
{
  public interface IReverseProxy
  {
      public Task<HttpResponseMessage> ProxyRequestToService(HttpContext context);
  }

  public class ReverseProxy : IReverseProxy
  {
    private readonly IGetServicesCached _getServices;
    private static readonly HttpClient _httpClient = new HttpClient();
    
    public ReverseProxy(IGetServicesCached getServices)
    {
      _getServices = getServices;
    }

    public async Task<HttpResponseMessage> ProxyRequestToService(HttpContext context)
    {
      
      var targetUri = BuildTargetUri(context.Request);
      if (targetUri == null) return new HttpResponseMessage {StatusCode = HttpStatusCode.NotFound};
      
      var targetRequestMessage = CreateTargetMessage(context, targetUri);
      using var responseMessage = await _httpClient.SendAsync(targetRequestMessage, HttpCompletionOption.ResponseHeadersRead, context.RequestAborted);
      context.Response.StatusCode = (int)responseMessage.StatusCode;
      CopyFromTargetResponseHeaders(context, responseMessage);
      await ProcessResponseContent(context, responseMessage);
      return responseMessage;
    }

    private Uri? BuildTargetUri(HttpRequest request)
    {
      var segments = request.Path.ToString().Split("/");
      var serviceRequest = segments.First();
      
      var remainingPath = "";
      if (segments.Length > 1)
      {
        var remaining = segments[1..^0];
        remainingPath = string.Join(",", remaining);
      }

      var services = _getServices.Execute();
      if (!services.ContainsKey(serviceRequest)) return null;
      var targetUri = new Uri(services[serviceRequest] + remainingPath);
      return targetUri;
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

    
    private async Task ProcessResponseContent(HttpContext context, HttpResponseMessage responseMessage)
    {
      var requestPath = context.Request.Path;
      var content = await responseMessage.Content.ReadAsByteArrayAsync();

      if (IsContentOfType(responseMessage, "text/html") || 
          IsContentOfType(responseMessage, "text/javascript"))
      {
        var stringContent = Encoding.UTF8.GetString(content);
        // TODO
        var newContent = stringContent.Replace("https://www.google.com", requestPath);
        await context.Response.WriteAsync(newContent, Encoding.UTF8);
      } else {
        await context.Response.Body.WriteAsync(content);
      }
    }
    
    private bool IsContentOfType(HttpResponseMessage responseMessage, string type)
    {
      var result = false;

      if (responseMessage.Content?.Headers?.ContentType != null)
      {
        result = responseMessage.Content.Headers.ContentType.MediaType == type;
      }

      return result;
    }
  }
}