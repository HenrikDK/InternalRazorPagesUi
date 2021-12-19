using System.Net;
using InternalRazorPagesUi.ReverseProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class ReverseProxyModel : PageModel
{
    public string ResponseBody { get; set; } = "";
    private readonly ILogger<ReverseProxyModel> _logger;
    private readonly IReverseProxy _reverseProxy;
    
    public ReverseProxyModel(ILogger<ReverseProxyModel> logger, IReverseProxy reverseProxy)
    {
        _logger = logger;
        _reverseProxy = reverseProxy;
    }
    
    public async Task<ActionResult> OnGet()
    {
        var response = await _reverseProxy.ProxyRequestToService(HttpContext);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new StatusCodeResult(404);
        }

        ResponseBody = response.Content.ToString() ?? "";
        ResponseBody = ExtractBody(ResponseBody);
        
        return Page();
    }

    public async Task<ActionResult> OnPost()
    {
        var response = await _reverseProxy.ProxyRequestToService(HttpContext);
        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return new StatusCodeResult(404);
        }

        ResponseBody = response.Content.ToString() ?? "";
        ResponseBody = ExtractBody(ResponseBody);

        return Page();
    }

    private string ExtractBody(string response)
    {
        if (string.IsNullOrEmpty(response)) return response;
        
        var body = response[(response.IndexOf("<body") + 6)..(response.IndexOf("</body") - 1)];
        return body;
    }
}
