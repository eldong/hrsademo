using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using System;
using System.Net;


namespace webapp.Pages;

public class SqlModel : PageModel
{
    private readonly ILogger<PrivacyModel> _logger;
    public string SqlDataText { get; set; }
    public string DNSLookup { get; set; }
    public string DNSLookupError { get; set; }
    public string SqlConnectionString { get; set; } = "";

    public string KeyVaultUrl { get; set; }




    public SqlModel(ILogger<PrivacyModel> logger)
    {
        _logger = logger;
    }

    public void OnGet()
    {
        GetSqlData();
    }

    public void GetSqlData()
    {
        string query = "SELECT TestData FROM TestTable";
        GetKeyVaultSecrets();   


        string host = "gssstgesqlmihrsa-est.2f99f90d19e1.database.windows.net";
        try
        {
            // Perform DNS lookup
            IPHostEntry hostEntry = Dns.GetHostEntry(host);

            DNSLookup = $"Host: {host}";
            DNSLookupError = "";
            foreach (IPAddress ip in hostEntry.AddressList)
            {
                Console.WriteLine($"IP Address: {ip}");
                DNSLookup += ip.ToString();
            }
        }
        catch (Exception ex)
        {
            DNSLookupError = ex.Message;
        }


        using (SqlConnection connection = new SqlConnection(SqlConnectionString))
        {
            try
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    SqlDataText = reader["TestData"].ToString();
                    Console.WriteLine(reader["TestData"]);
                }

                reader.Close();
            }
            catch (Exception e)
            {
                SqlDataText = e.Message;
                return;
            }
        }
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
            secret = client.GetSecret("gssstgewebapp-sqlconn");
            SqlConnectionString = secret.Value;
            return "success";
        }
        catch (Exception e)
        {
            SqlConnectionString = "";
            return e.Message;
        }
    }


    public IActionResult OnPost()
    {
        // Set the value of MyTextBoxProperty here
        SqlDataText = "This should work";
        return Page();
    }
    
}

