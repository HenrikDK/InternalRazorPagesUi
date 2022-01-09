using InternalRazorPagesUi.Model.Repositories;

namespace InternalRazorPagesUi.Pages;

public class ColumnFilterModel : PageModel
{
    private readonly ILogger<ColumnFilterModel> _logger;
    private readonly IItemRepository _itemRepository;

    public ColumnFilterModel(ILogger<ColumnFilterModel> logger, IItemRepository itemRepository)
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
