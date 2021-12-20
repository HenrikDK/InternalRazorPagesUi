using InternalRazorPagesUi.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class BreakoutModel : PageModel
{
    private readonly ILogger<BreakoutModel> _logger;
    private readonly IGetItems _getItems;

    public BreakoutModel(ILogger<BreakoutModel> logger, IGetItems getItems)
    {
        _logger = logger;
        _getItems = getItems;
    }

    public JsonResult OnGetItems()
    {
        var items = _getItems.GetAll();
        return new JsonResult(items);
    }
    
    public void OnGet()
    {
    }
}
