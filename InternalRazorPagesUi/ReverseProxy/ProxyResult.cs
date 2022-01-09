namespace InternalRazorPagesUi.ReverseProxy;

public class ProxyResult
{
    public ContentType ContentType { get; set; }
    public string Title { get; set; }
    public HtmlString Body { get; set; }
    public string Content { get; set; }
    public byte[] Bytes { get; set; }
}