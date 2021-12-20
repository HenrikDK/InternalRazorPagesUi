using InternalRazorPagesUi.Model;
using InternalRazorPagesUi.Model.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class SearchModel : PageModel
{
    private readonly ILogger<SearchModel> _logger;
    private readonly IItemRepository _itemRepository;

    public SearchModel(ILogger<SearchModel> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public JsonResult OnGetPirates()
    {
        var items = _itemRepository.GetAll();
        return new JsonResult(items);
    }
    
    public JsonResult OnGetLobsters()
    {
        var items = _itemRepository.GetAll();
        return new JsonResult(items);
    }
    
    public void OnGet()
    {
    }
}
