using InternalRazorPagesUi.Model.Repositories;

namespace InternalRazorPagesUi.Pages;

public class MultiModel : PageModel
{
    private readonly ILogger<MultiModel> _logger;
    private readonly IItemRepository _itemRepository;

    public MultiModel(ILogger<MultiModel> logger, IItemRepository itemRepository)
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
