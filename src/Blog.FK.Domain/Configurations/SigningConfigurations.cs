using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;

namespace Blog.FK.Domain.Configurations
{
    /// <summary>
    /// Define Sign In SecurityKey and Credentials
    /// </summary>
    public class SigningConfigurations
    {
        /// <summary>
        /// Rsa Security Key
        /// </summary>
        public SecurityKey SecurityKey { get; set; }

        /// <summary>
        /// RsaSha 256 Signature Credentials
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }

        public SigningConfigurations()
        {
            using (var provider = new RSACryptoServiceProvider(2048))
            {
                SecurityKey = new RsaSecurityKey(provider.ExportParameters(true));
            }

            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.RsaSha256Signature);
        }
    }
}
