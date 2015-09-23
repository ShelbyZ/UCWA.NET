using System.Collections.Generic;
using System.Threading.Tasks;

namespace UCWA.NET.Transport
{
    public class TransportProxy
    {
        private ITransport _transport;

        public string Authorization { get; set; }

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

            if (!request.Headers.ContainsKey("Authorization"))
            {
                request.Headers.Add("Authorization", Authorization);
            }

            return _transport.ExecuteRequest(request);
        }

        public async Task<Response> ExecuteRequestAsync(Request request)
        {
            return await Task.Run<Response>(() =>
            {
                return ExecuteRequest(request);
            });
        }
    }
}
