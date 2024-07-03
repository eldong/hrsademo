using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace webapp.Pages;

public class SqlModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    public string SqlDataText { get; set; }

    public SqlModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
    }

    public IActionResult OnPost()
    {
        // Set the value of MyTextBoxProperty here
        SqlDataText = "This should work";
        return Page();
    }
    
}

