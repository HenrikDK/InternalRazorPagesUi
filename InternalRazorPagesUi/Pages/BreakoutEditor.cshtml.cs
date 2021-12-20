using System.Transactions;
using InternalRazorPagesUi.Model;
using InternalRazorPagesUi.Model.Repositories;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class BreakoutEditorModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public Item Item { get; set; }
    [BindProperty(SupportsGet = true)]
    public EditorMode EditMode { get; set; }

    private readonly ILogger<BreakoutEditorModel> _logger;
    private readonly IItemRepository _itemRepository;

    public BreakoutEditorModel(ILogger<BreakoutEditorModel> logger, IItemRepository itemRepository)
    {
        _logger = logger;
        _itemRepository = itemRepository;
    }
    
    public IActionResult OnGet(string url)
    {
        SetEditMode(Request.GetDisplayUrl());

        if (EditMode is EditorMode.Edit or EditorMode.Delete)
        {
            var id = url.Split("?").First();
            Item = _itemRepository.GetBy(int.Parse(id));
        }

        if (EditMode is EditorMode.New)
        {
            Item = new Item();
            var queryString = Request.QueryString.Value ?? "";
            if (!queryString.Contains("copyFrom")) return Page();
            
            var copyFromId = queryString[queryString.IndexOf("copyFrom")..];
            if (copyFromId.Contains("&"))
            {
                copyFromId = copyFromId[..copyFromId.IndexOf("&")];
            }
            copyFromId = copyFromId.Split("=")[1];
            Item = _itemRepository.GetBy(int.Parse(copyFromId));
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

    public IActionResult OnPost(string url)
    {
        SetEditMode(Request.GetDisplayUrl());

        if (EditMode is EditorMode.Delete)
        {
            using var scope = new TransactionScope();
            _itemRepository.Delete(Item.Id);
            scope.Complete();
            return RedirectToPage("Breakout");
        }

        if (!ModelState.IsValid)
        {
            return Page();
        }

        using (var scope = new TransactionScope())
        {
            if (EditMode == EditorMode.New)
            {
                _itemRepository.Insert(Item);
            }
            else
            {
                _itemRepository.Update(Item);
            }
         
            scope.Complete();
        }
        
        return RedirectToPage("Breakout");
    }
}
