using System;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using UCWA.NET.Resources;
using UCWA.NET.Transport;

namespace UCWA.NET.Core
{
    public class Discovery
    {
        private TransportProxy _proxy;

        public Discovery(TransportProxy proxy)
        {
            _proxy = proxy;
        }

        public async Task<Autodiscover> Start(string domainOrUsername)
        {
            var domain = domainOrUsername;

            try
            {
                domain = new MailAddress(domainOrUsername).Host;
            }
            catch (FormatException)
            {
            }

            var response = await DiscoverSection(true, domain);
            if (response == null || (response != null && response.Data == null))
            {
                response = await DiscoverSection(false, domain);
            }

            if (response != null && response.Data != null)
            {
                return await FollowRedirects(response.Data.FromBytes<Autodiscover>());
            }
            else
            {
                throw new DiscoveryException(string.Format("Discovery was unable to find resource for domain: {0}", domain));
            }
        }

        private async Task<Response> DiscoverSection(bool isInternal, string domain)
        {
            var results = await Task.WhenAll(Discover("https", isInternal, domain), Discover("http", isInternal, domain));
            foreach (var item in results)
            {
                if (item != null)
                {
                    return item;
                }
            }

            return null;
        }

        private async Task<Response> Discover(string scheme, bool isInternal, string domain)
        {
            return await _proxy.ExecuteRequestAsync(new Request
            {
                Uri = new Uri(string.Format("{0}://lyncdiscover{1}.{2}", scheme, isInternal ? "internal" : string.Empty, domain)),
                Method = HttpMethod.Get
            });
        }

        private async Task<Autodiscover> FollowRedirects(Autodiscover autodiscover)
        {
            while (autodiscover.Links.Redirect != null)
            {
                var response = await _proxy.ExecuteRequestAsync(new Request
                {
                    Uri = new Uri(autodiscover.Links.Redirect.Href, UriKind.Relative),
                    Method = HttpMethod.Get
                });

                if (response == null)
                {
                    throw new DiscoveryException(string.Format("Discovery followed redirect {0} and received null response", autodiscover.Links.Redirect.Href));
                }

                autodiscover = response?.Data?.FromBytes<Autodiscover>();
            }

            return autodiscover;
        }
    }
}
