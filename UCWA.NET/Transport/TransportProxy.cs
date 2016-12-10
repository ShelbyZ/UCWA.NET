using System;
using System.Collections.Generic;
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

        public Response ExecuteRequest(Request request)
        {
            if (request.Headers == null)
            {
                request.Headers = new Dictionary<string, string>();
            }

            if (!request.Headers.ContainsKey("Authorization") && Authorization != null)
            {
                request.Headers.Add("Authorization", Authorization);
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

            return _transport.ExecuteRequest(request);
        }

        public async Task<Response> ExecuteRequestAsync(Request request)
        {
            return await Task.Run(() =>
            {
                return ExecuteRequest(request);
            });
        }
    }
}
