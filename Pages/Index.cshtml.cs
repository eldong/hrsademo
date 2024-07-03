using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;


namespace webapp.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    public string KeyVaultUrl { get; set; }
    public string KeyVaultSecret { get; set; }
    public string KVMessage { get; set; }

    public IndexModel(ILogger<IndexModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        KVMessage = GetKeyVaultSecrets();
    }

    private string GetKeyVaultSecrets()
    {
       
        if (Environment.GetEnvironmentVariable("WEBAPPLOCATION") == "HRSADEMO")
        {
            KeyVaultUrl = "https://hrsademo.vault.azure.net/";
        }
        else
        {
            KeyVaultUrl = "https://gssstgewebapp-kv.vault.azure.net/";
            // KeyVaultUrl = "https://gssstgewebapp-kv.privatelink.vaultcore.azure.net/";
        }
       
       
        //KeyVaultUrl = Environment.GetEnvironmentVariable("KEYVAULT_URL");
        

        //blobContainerName = Environment.GetEnvironmentVariable("BLOBCONTAINERNAME");

        var client = new SecretClient(new Uri(KeyVaultUrl), new DefaultAzureCredential());
        KeyVaultSecret secret;
        try
        {
            // secret = client.GetSecret("SECRETONE");
            secret = client.GetSecret("gssstgewebapp-user");
            KeyVaultSecret = secret.Value;
            return "success";
        }
        catch (Exception e)
        {
            return e.Message;
        }
    }

}
