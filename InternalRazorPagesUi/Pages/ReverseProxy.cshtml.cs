using System.Net;
using InternalRazorPagesUi.ReverseProxy;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class ReverseProxyModel : PageModel
{
    public string Title { get; set; }
    public HtmlString ResponseBody { get; set; }
    
    private readonly ILogger<ReverseProxyModel> _logger;
    private readonly IProcessResponse _processResponse;
    private readonly IProxyRequest _proxyRequest;
    
    public ReverseProxyModel(ILogger<ReverseProxyModel> logger,
        IProcessResponse processResponse,
        IProxyRequest proxyRequest)
    {
        _logger = logger;
        _processResponse = processResponse;
        _proxyRequest = proxyRequest;
    }
    
    public async Task<ActionResult> OnGet()
    {
        var (proxyRequest, response) = await _proxyRequest.RequestFromService(HttpContext);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new StatusCodeResult(404);
        }
        
        if (response.StatusCode == HttpStatusCode.Found)
        {
            var relativePath = Flurl.Url.Combine(proxyRequest.RequestServicePath, response.Headers.Location?.OriginalString);
            return new RedirectResult(relativePath);
        }

        var result = await _processResponse.ProcessAndExtract(HttpContext, response, proxyRequest);
        if (result.ContentType == ContentType.TextHtml)
        {
            Title = result.Title;
            ResponseBody = result.Body;
            return Page();
        }

        if (result.ContentType == ContentType.TextJs)
        {
            return new FileContentResult(result.Bytes, "text/javascript");    
        }

        return new FileContentResult(result.Bytes, response.Content.Headers.ContentType?.MediaType ?? "text/json");
    }

    public async Task<ActionResult> OnPost()
    {
        var (proxyRequest, response) = await _proxyRequest.RequestFromService(HttpContext);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new StatusCodeResult(404);
        }

        if (response.StatusCode == HttpStatusCode.Found)
        {
            var relativePath = Flurl.Url.Combine(proxyRequest.RequestServicePath, response.Headers.Location?.OriginalString);
            return new RedirectResult(relativePath);
        }

        var result = await _processResponse.ProcessAndExtract(HttpContext, response, proxyRequest);
        if (result.ContentType == ContentType.TextHtml)
        {
            Title = result.Title;
            ResponseBody = result.Body;
            return Page();
        }

        if (result.ContentType == ContentType.TextJs)
        {
            return new FileContentResult(result.Bytes, "text/javascript");    
        }

        return new FileContentResult(result.Bytes, response.Content.Headers.ContentType?.MediaType ?? "text/json");
    }
}
