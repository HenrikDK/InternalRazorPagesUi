using InternalRazorPagesUi.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace InternalRazorPagesUi.Pages;

public class BreakoutEditorModel : PageModel
{
    private readonly ILogger<BreakoutEditorModel> _logger;

    public BreakoutEditorModel(ILogger<BreakoutEditorModel> logger)
    {
        _logger = logger;
    }

    public EditorMode EditMode { get; set; }

    public IActionResult OnGet(string url)
    {
        EditMode = EditorMode.Edit;
        
        return Page();
    }
}
