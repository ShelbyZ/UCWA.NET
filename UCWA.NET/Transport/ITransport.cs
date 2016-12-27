using System;
using System.Threading.Tasks;

namespace UCWA.NET.Transport
{
    public interface ITransport
    {
        [Obsolete("Use ExecuteRequestAsync(...)")]
        Response ExecuteRequest(Request request);

        Task<Response> ExecuteRequestAsync(Request request);
    }
}
