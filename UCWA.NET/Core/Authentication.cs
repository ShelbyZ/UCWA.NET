using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UCWA.NET.Resources;
using UCWA.NET.Transport;

namespace UCWA.NET.Core
{
    public class Authentication
    {
        private TransportProxy _proxy;

        public Authentication(TransportProxy proxy)
        {
            _proxy = proxy;
        }

        public async Task<Root> Start(Uri uri, AuthToken authToken = null)
        {
            if (authToken != null)
            {
                _proxy.Authorization = string.Format("{0} {1}", authToken.TokenType, authToken.AccessToken);
            }

            var response = await _proxy.ExecuteRequestAsync(new Request
            {
                Uri = uri,
                Method = HttpMethod.Get
            });
            if (response?.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new AuthenticationException(new Challenge(response.Headers[Constants.WwwAuthenticate]), "Authentication failed due to challenge");
            }
            else
            {
                var root = response?.Data?.FromBytes<Root>();
                if (root != null)
                {
                    root.Baseuri = new Uri(new Uri(root.Links.Self.Href).GetLeftPart(UriPartial.Authority));
                    _proxy.Baseuri = root.Baseuri;
                    return root;
                }
            }

            throw new AuthenticationException(null, "Authentication was unable to start properly");
        }

        public async Task<AuthToken> GetAuthToken(Uri uri, Credentials credentials)
        {
            var request = new Request
            {
                Uri = uri,
                Method = HttpMethod.Post,
                Headers = new Dictionary<string, string>
                {
                    { Constants.ContentType, Constants.XWwwFormUrlencoded }
                },
                Data = GetGrant(credentials)
            };

            if (credentials.GrantType == Constants.Windows)
            {
                var cache = new CredentialCache();
                cache.Add(uri, "NTLM", new NetworkCredential(credentials.Username, credentials.Password, credentials.Domain));
                request.Credentials = cache;
            }

            var response = await _proxy.ExecuteRequestAsync(request);
            return response?.Data?.FromBytes<AuthToken>();
        }

        private byte[] GetGrant(Credentials credentials)
        {
            var grant = string.Format("grant_type={0}", credentials.GrantType);

            if (credentials.GrantType == Constants.Password)
            {
                grant = string.Format("{0}&username={1}&password={2}", grant, Uri.EscapeUriString(credentials.Username), Uri.EscapeUriString(credentials.Password));
            }

            // TODO: support online meeting join
            return Encoding.UTF8.GetBytes(grant);
        }
    }
}
