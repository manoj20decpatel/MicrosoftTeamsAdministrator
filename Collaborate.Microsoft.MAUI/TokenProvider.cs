using Microsoft.Kiota.Abstractions.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collaborate.Microsoft.MAUI
{
    public class TokenProvider : IAccessTokenProvider
    {
        string strToken = string.Empty;
        public TokenProvider(string token)
        {
            strToken = token;
        }

        public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object> additionalAuthenticationContext = default,
            CancellationToken cancellationToken = default)
        {
            var token = strToken;
            // get the token and return it
            return Task.FromResult(token);
        }

        public AllowedHostsValidator AllowedHostsValidator { get; }
    }
}
