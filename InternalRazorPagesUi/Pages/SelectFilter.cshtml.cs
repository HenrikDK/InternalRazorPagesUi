using InternalRazorPagesUi.Model;
using InternalRazorPagesUi.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class ColumnFilterModel : PageModel
{
    private readonly ILogger<ColumnFilterModel> _logger;
    private readonly IGetItems _getItems;

    public ColumnFilterModel(ILogger<ColumnFilterModel> logger, IGetItems getItems)
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
