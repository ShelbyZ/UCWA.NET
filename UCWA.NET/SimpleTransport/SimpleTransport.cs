using System.Net;
using UCWA.NET.Transport;

namespace UCWA.NET.SimpleTransport
{
    public class SimpleTransport : ITransport
    {
        public Response ExecuteRequest(Request request)
        {
            var temp = WebRequest.CreateHttp(request.Uri);

            if (request.Headers != null)
            {
                foreach (var item in request.Headers)
                {
                    temp.Headers.Add(item.Key, item.Value);
                }
            }

            if (request.Data != null)
            {
                using (var stream = temp.GetRequestStream())
                {
                    if (stream != null)
                    {
                        stream.Write(request.Data, 0, request.Data.Length);
                    }
                }
            }

            try
            {
                var result = temp.GetResponse() as HttpWebResponse;
                var response = new Response
                {
                    Uri = result.ResponseUri,
                    StatusCode = result.StatusCode,
                };

                using (var stream = result.GetResponseStream())
                {
                    if (stream != null && result.ContentLength != 0)
                    {
                        var bytes = new byte[result.ContentLength];
                        stream.Read(bytes, 0, (int)result.ContentLength);

                        response.Data = bytes;
                    }
                }

                foreach (var item in result.Headers.AllKeys)
                {
                    response.Headers.Add(item, result.Headers[item]);
                }

                return response;
            }
            catch(WebException)
            {
                return null;
            }
        }
    }
}
