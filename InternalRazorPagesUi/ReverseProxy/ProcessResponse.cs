namespace InternalRazorPagesUi.ReverseProxy;

public interface IProcessResponse
{
  public Task<ProxyResult> ProcessAndExtract(HttpContext context, HttpResponseMessage responseMessage, ProxyPath proxyRequest);
}
  
public class ProcessResponse : IProcessResponse
{
  private static readonly Regex FormActions = new Regex("<\\s*form[^>]*\\s+action=([\"'])(.*?)\\1", RegexOptions.IgnoreCase);
  private static readonly Regex ButtonActions = new Regex("<\\s*button[^>]*\\s+formaction=([\"'])(.*?)\\1", RegexOptions.IgnoreCase);
  private static readonly Regex InputActions = new Regex("<\\s*input[^>]*\\s+formaction=([\"'])(.*?)\\1", RegexOptions.IgnoreCase);
  private static readonly Regex Links = new Regex("<\\s*a[^>]*\\s+href=([\"'])(.*?)\\1", RegexOptions.IgnoreCase);
  private static readonly Regex Images = new Regex("<\\s*img[^>]*\\s+src=([\"'])(.*?)\\1", RegexOptions.IgnoreCase);
    
  public async Task<ProxyResult> ProcessAndExtract(HttpContext context, HttpResponseMessage responseMessage, ProxyPath proxyRequest)
  {
    var result = new ProxyResult();
    var bytes = await responseMessage.Content.ReadAsByteArrayAsync();
    result.Bytes = bytes;

    var contentType = GetContentType(responseMessage);
    result.ContentType = contentType;

    if (contentType is ContentType.TextHtml or ContentType.TextJs)
    {
      var content = Encoding.UTF8.GetString(bytes);
      if (string.IsNullOrEmpty(content)) return result;

      var requestPath = "/" + proxyRequest.ControllerPrefixPath + proxyRequest.RequestPath;
      content = content.Replace(Url.Combine(proxyRequest.ServiceAddress, proxyRequest.ServicePath), requestPath);
      content = content.Replace(Url.Combine("http://localhost", proxyRequest.ServicePath), requestPath);

      content = Replace(FormActions, proxyRequest, content);
      content = Replace(ButtonActions, proxyRequest, content);
      content = Replace(InputActions, proxyRequest, content);
      content = Replace(Links, proxyRequest, content);
      content = Replace(Images, proxyRequest, content);

      var title = content[(content.IndexOf("<title") + 7)..(content.IndexOf("</title") - 1)];
      var body = content[(content.IndexOf("<body") + 6)..(content.IndexOf("</body") - 1)];

      result.Body = new HtmlString(body);
      result.Title = title;
      result.Content = content;
    }
      
    return result;
  }

  private static string Replace(Regex regex, ProxyPath proxyRequest, string content)
  {
    var matches = regex.Matches(content);
      
    if (!matches.Any()) return content;

    var sb = new StringBuilder(content);
      
    for (var i = matches.Count - 1; i >= 0; i--)
    {
      var original = matches[i].Groups[2].Value;
      if (original.IndexOf("/") != 0) continue;
      var index = matches[i].Groups[2].Index;
      var length = matches[i].Groups[2].Length;
      var replacement = Url.Combine(proxyRequest.RequestServicePath, original);

      sb.Remove(index, length)
        .Insert(index, replacement);
    }
    return sb.ToString();
  }

  private ContentType GetContentType(HttpResponseMessage responseMessage)
  {
    if(IsContentOfType(responseMessage, "text/html"))
    {
      return ContentType.TextHtml;
    }

    if (IsContentOfType(responseMessage, "text/javascript"))
    {
      return ContentType.TextJs;
    }

    return ContentType.Unknown;
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