using Microsoft.Azure.KeyVault.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;

namespace SecretsDemo
{
    public class SecretManager : IKeyVaultSecretManager
    {

        public SecretManager()
        {
        }

        public bool Load(SecretItem secret)
        {
            return true;
        }

        public string GetKey(SecretBundle secret)
        {
            return secret.SecretIdentifier.Name;
        }
    }
}
