using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace UCWA.NET.Transport
{
    public class TransportProxy
    {
        private ITransport _transport;

        public string Authorization { get; set; }

        public Uri Baseuri { get; set; }

        public TransportProxy(ITransport transport)
        {
            _transport = transport;
        }

        [Obsolete("Use ExecuteRequestAsync(...)")]
        public Response ExecuteRequest(Request request)
        {
            PrepareRequest(request);

            return _transport.ExecuteRequest(request);
        }

        public async Task<Response> ExecuteRequestAsync(Request request)
        {
            PrepareRequest(request);

            return await _transport.ExecuteRequestAsync(request);
        }

        private void PrepareRequest(Request request)
        {
            if (request.Headers == null)
            {
                request.Headers = new Dictionary<string, string>();
            }

            if (!request.Headers.ContainsKey(HttpRequestHeader.Authorization.ToString()) && Authorization != null)
            {
                request.Headers.Add(HttpRequestHeader.Authorization.ToString(), Authorization);
            }

            if (!request.Uri.IsAbsoluteUri)
            {
                request.Uri = new Uri(Baseuri, request.Uri);
            }

            if (request.Timeout == default(int))
            {
                // Default HttpWebRequest.Timeout value
                request.Timeout = 100000;
            }
        }
    }
}
