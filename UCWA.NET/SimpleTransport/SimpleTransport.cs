using System.Net;
using UCWA.NET.Transport;

namespace UCWA.NET.SimpleTransport
{
    public class SimpleTransport : ITransport
    {
        public Response ExecuteRequest(Request request)
        {
            var obj = WebRequest.CreateHttp(request.Uri);
            obj.Method = request.Method.ToString();
            obj.Credentials = request?.Credentials;
            obj.Timeout = request.Timeout;

            if (request.Headers != null)
            {
                foreach (var item in request?.Headers)
                {
                    if (item.Key == "Content-Type")
                    {
                        obj.ContentType = item.Value;
                    }
                    else
                    {
                        obj.Headers.Add(item.Key, item.Value);
                    }
                }
            }

            if (request.Data != null)
            {
                using (var stream = obj.GetRequestStream())
                {
                    stream?.Write(request.Data, 0, request.Data.Length);
                }
            }

            try
            {
                return ProcessResponse(obj.GetResponse() as HttpWebResponse);
            }
            catch(WebException ex)
            {
                return ex.Response != null ? ProcessResponse(ex.Response as HttpWebResponse) : null;
            }
        }

        public Response ProcessResponse(HttpWebResponse response)
        {
            var obj = new Response
            {
                Uri = response.ResponseUri,
                StatusCode = response.StatusCode,
            };

            using (var stream = response.GetResponseStream())
            {
                if (stream != null && response.ContentLength > 0)
                {
                    var bytes = new byte[response.ContentLength];
                    stream.Read(bytes, 0, (int)response.ContentLength);

                    obj.Data = bytes;
                }
            }

            foreach (var item in response.Headers.AllKeys)
            {
                obj.Headers.Add(item, response.Headers[item]);
            }

            return obj;
        }
    }
}
