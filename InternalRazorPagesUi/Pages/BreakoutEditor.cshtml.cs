using InternalRazorPagesUi.Model;
using InternalRazorPagesUi.Model.Queries;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class BreakoutEditorModel : PageModel
{
    public Item Item { get; set; }
    private readonly ILogger<BreakoutEditorModel> _logger;
    private readonly IGetItems _getItems;

    public BreakoutEditorModel(ILogger<BreakoutEditorModel> logger, IGetItems getItems)
    {
        _logger = logger;
        _getItems = getItems;
    }

    public EditorMode EditMode { get; set; }

    public IActionResult OnGet(string url)
    {
        SetEditMode(url);

        if (EditMode is EditorMode.Edit or EditorMode.Delete)
        {
            var id = url.Split("?").First();
            Item = _getItems.GetBy(int.Parse(id));
        }

        if (EditMode is EditorMode.New)
        {
            Item = new Item();
            if (!url.Contains("copyFrom")) return Page();
            
            var copyFromId = url[url.IndexOf("copyFrom")..];
            if (copyFromId.Contains("&"))
            {
                copyFromId = copyFromId[..copyFromId.IndexOf("&")];
            }
            copyFromId = copyFromId.Split("=")[1];
            Item = _getItems.GetBy(int.Parse(copyFromId));
            Item.Id = 0;
            Item.isDelete = false;
        }

        return Page();
    }

    private void SetEditMode(string url)
    {
        EditMode = EditorMode.Edit;
        if (url.Contains("new"))
        {
            EditMode = EditorMode.New;
        }

        if (url.Contains("handler=delete"))
        {
            EditMode = EditorMode.Delete;
        }
    }

    public void OnPost(string url)
    {
    }
}
