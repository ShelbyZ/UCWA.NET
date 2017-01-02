using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using UCWA.NET.Transport;

namespace UCWA.NET.SimpleTransport
{
    public class SimpleTransport : ITransport
    {
        public Response ExecuteRequest(Request request)
        {
            var obj = CreateRequest(request);

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
            catch (WebException ex)
            {
                return ProcessResponse(ex.Response as HttpWebResponse);
            }
        }

        public async Task<Response> ExecuteRequestAsync(Request request)
        {
            var obj = CreateRequest(request);

            if (request.Data != null)
            {
                using (var stream = await obj.GetRequestStreamAsync())
                {
                    await stream?.WriteAsync(request.Data, 0, request.Data.Length);
                }
            }

            try
            {
                return await ProcessResponseAsync(await obj.GetResponseAsync() as HttpWebResponse);
            }
            catch (WebException ex)
            {
                return await ProcessResponseAsync(ex.Response as HttpWebResponse);
            }
        }

        private HttpWebRequest CreateRequest(Request request)
        {
            var obj = WebRequest.CreateHttp(request.Uri);
            obj.Method = request.Method.ToString();
            obj.Credentials = request?.Credentials;
            obj.Timeout = request.Timeout;

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

            if (request.Method == HttpMethod.Post && request.Data == null)
            {
                obj.ContentLength = 0;
            }

            return obj;
        }

        private Response ProcessResponse(HttpWebResponse response)
        {
            // Quick exit if the response object is null
            if (response == null)
            {
                return null;
            }

            var obj = new Response
            {
                Uri = response.ResponseUri,
                StatusCode = response.StatusCode,
            };

            using (var stream = response.GetResponseStream())
            {
                if (stream != null && response.ContentLength > 0)
                {
                    obj.Data = new byte[response.ContentLength];
                    var pos = 0;
                    while (pos < obj.Data.Length)
                    {
                        pos += stream.Read(obj.Data, pos, obj.Data.Length - pos);
                    }
                }
            }

            foreach (var item in response.Headers.AllKeys)
            {
                obj.Headers.Add(item, response.Headers[item]);
            }

            response.Close();

            return obj;
        }

        private async Task<Response> ProcessResponseAsync(HttpWebResponse response)
        {
            // Quick exit if the response object is null
            if (response == null)
            {
                return null;
            }

            var obj = new Response
            {
                Uri = response.ResponseUri,
                StatusCode = response.StatusCode,
            };

            using (var stream = response.GetResponseStream())
            {
                if (stream != null && response.ContentLength > 0)
                {
                    obj.Data = new byte[response.ContentLength];
                    var pos = 0;
                    while (pos < obj.Data.Length)
                    {
                        pos += await stream.ReadAsync(obj.Data, pos, obj.Data.Length - pos);
                    }
                }
            }

            foreach (var item in response.Headers.AllKeys)
            {
                obj.Headers.Add(item, response.Headers[item]);
            }

            response.Close();

            return obj;
        }
    }
}
