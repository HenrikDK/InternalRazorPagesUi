using InternalRazorPagesUi.Model;
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

    public JsonResult OnPost([FromBody] IList<Item> items)
    {
        var existing = _itemRepository.GetAll();

        var updates = items.Where(x => existing.Any(y => y.Id == x.Id && !y.Equals(x))).ToList();
        var inserts = items.Where(x => existing.All(y => y.Id != x.Id)).ToList();
        var deletes = items.Where(x => existing.Any(y => y.Id == x.Id && x.isDelete && x.isDelete != y.isDelete))
            .ToList();
        var restores = items.Where(x => existing.Any(y => y.Id == x.Id && !x.isDelete && x.isDelete != y.isDelete))
            .ToList();

        using (var scope = new TransactionScope())
        {
            if (deletes.Count > 0)
            {
                _itemRepository.DeleteAll(deletes);
            }

            if (updates.Count > 0)
            {
                _itemRepository.UpdateAll(updates);
            }

            if (inserts.Count > 0)
            {
                _itemRepository.InsertAll(inserts);
            }

            if (restores.Count > 0)
            {
                _itemRepository.RestoreAll(restores);
            }

            scope.Complete();
        }

        return new JsonResult(new {success = true});
    }
}