using InternalRazorPagesUi.Model;
using InternalRazorPagesUi.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class MultiModel : PageModel
{
    private readonly ILogger<MultiModel> _logger;
    private readonly IGetItems _getItems;

    public MultiModel(ILogger<MultiModel> logger, IGetItems getItems)
    {
        _logger = logger;
        _getItems = getItems;
    }

    public JsonResult OnGetPirates()
    {
        var items = _getItems.GetAll();
        return new JsonResult(items);
    }
    
    public JsonResult OnGetLobsters()
    {
        var items = _getItems.GetAll();
        return new JsonResult(items);
    }
    
    public void OnGet()
    {
    }
}
