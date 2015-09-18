using System;
using System.Net.Http;
using UCWA.NET.Resources;
using UCWA.NET.SimpleTransport;
using UCWA.NET.Transport;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var proxy = new TransportProxy(new SimpleTransport());
            var response = proxy.ExecuteRequest(new Request
            {
                Uri = new Uri("http://lyncdiscover.metio.net"),
                Method = HttpMethod.Get
            });

            if (response != null)
            {
                var autodiscovery = response.Data.FromBytes<Autodiscovery>();
                if (autodiscovery != null)
                {

                }
            }
        }
    }
}
