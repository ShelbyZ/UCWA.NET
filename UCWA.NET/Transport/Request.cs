using System.Net.Http;

namespace UCWA.NET.Transport
{
    public class Request : WebAction
    {
        public HttpMethod Method { get; set; }
    }
}
