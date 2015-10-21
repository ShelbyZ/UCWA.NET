using System.Net;
using System.Net.Http;

namespace UCWA.NET.Transport
{
    public class Request : WebAction
    {
        public HttpMethod Method { get; set; }

        public ICredentials Credentials { get; set; }

        public int Timeout { get; set; }
    }
}
