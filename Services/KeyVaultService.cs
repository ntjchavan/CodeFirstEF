using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace CodeFirstEFAPI.Services
{
    public class KeyVaultService
    {
        private readonly SecretClient _secretClient;

        public KeyVaultService(string keyVaultUrl)
        {
            _secretClient = new SecretClient(new Uri(keyVaultUrl), new DefaultAzureCredential());
        }

        public async Task<string> GetSecretAsync(string secretKey)
        {
            KeyVaultSecret keyVaultSecret = await _secretClient.GetSecretAsync(secretKey);

            return keyVaultSecret.Value;
        }
    }
}
