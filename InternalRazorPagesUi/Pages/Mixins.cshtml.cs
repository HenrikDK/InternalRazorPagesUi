namespace InternalRazorPagesUi.Pages;

public class MixinsModel : PageModel
{
    private readonly ILogger<MixinsModel> _logger;

    public MixinsModel(ILogger<MixinsModel> logger)
    {
        _logger = logger;
    }
    
    public void OnGet()
    {
    }
}
