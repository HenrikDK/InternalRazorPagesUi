using InternalRazorPagesUi.Model.Repositories;

namespace InternalRazorPagesUi.Pages;

public class InlineModel : PageModel
{
    private readonly ILogger<InlineModel> _logger;
    private readonly IItemRepository _itemRepository;

    public InlineModel(ILogger<InlineModel> logger, IItemRepository itemRepository)
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
