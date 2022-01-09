using MicroServiceUi.Model.Repositories;

namespace MicroServiceUi.Pages;

public class SelectModel : PageModel
{
    private readonly ILogger<SelectModel> _logger;
    private readonly IItemRepository _itemRepository;

    public SelectModel(ILogger<SelectModel> logger, IItemRepository itemRepository)
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
