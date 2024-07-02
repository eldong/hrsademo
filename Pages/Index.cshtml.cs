using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;


namespace webapp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string KeyVaultSecret { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        GetKeyVaultSecrets();

    }

    private string GetKeyVaultSecrets()
    {
        //var kvUrl = Environment.GetEnvironmentVariable("KEYVAULT_URL");
        var kvUrl = "https://hrsademo.vault.azure.net/";
        //blobContainerName = Environment.GetEnvironmentVariable("BLOBCONTAINERNAME");

        var client = new SecretClient(new Uri(kvUrl), new DefaultAzureCredential());
        KeyVaultSecret secret;
        try
        {
            secret = client.GetSecret("SECRETONE");
            KeyVaultSecret = secret.Value;
            return "success";
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

}