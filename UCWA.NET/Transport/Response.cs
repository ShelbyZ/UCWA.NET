using System.Net;

namespace UCWA.NET.Transport
{
    public class Response : WebAction
    {
        public HttpStatusCode StatusCode { get; set; }
    }
}
