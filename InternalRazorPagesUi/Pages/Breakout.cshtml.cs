using InternalRazorPagesUi.Model.Repositories;

namespace InternalRazorPagesUi.Pages;

public class BreakoutModel : PageModel
{
    private readonly ILogger<BreakoutModel> _logger;
    private readonly IItemRepository _itemRepository;

    public BreakoutModel(ILogger<BreakoutModel> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }

    public JsonResult OnGetItems()
    {
        var items = _itemRepository.GetAll();
        return new JsonResult(items);
    }
    
    public void OnGet()
    {
    }
}
