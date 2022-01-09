using InternalRazorPagesUi.Model.Repositories;

namespace InternalRazorPagesUi.Pages;

public class ControlBarModel : PageModel
{
    private readonly ILogger<ControlBarModel> _logger;
    private readonly IItemRepository _itemRepository;

    public ControlBarModel(ILogger<ControlBarModel> logger, IItemRepository itemRepository)
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
