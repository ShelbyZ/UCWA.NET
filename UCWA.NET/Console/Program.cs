using System;
using System.Collections.Generic;
using System.Net.Http;
using UCWA.NET.Core;
using UCWA.NET.Resources;
using UCWA.NET.SimpleTransport;
using UCWA.NET.Transport;

namespace Console
{
    class Program
    {
        static TransportProxy _proxy;
        static Discovery _discovery;
        static Authentication _authentication;

        static void Main(string[] args)
        {
            _proxy = new TransportProxy(new SimpleTransport());
            _discovery = new Discovery(_proxy);
            _authentication = new Authentication(_proxy);

            System.Console.WriteLine("Enter discovery domain");
            var discoveryDomain = System.Console.ReadLine();
            System.Console.WriteLine("Enter username");
            var username = System.Console.ReadLine();
            System.Console.WriteLine("Enter password");
            var password = System.Console.ReadLine();

            System.Console.WriteLine("Starting discovery...");
            var discoveryTask = _discovery.Start(!string.IsNullOrWhiteSpace(discoveryDomain) ? discoveryDomain : username);
            discoveryTask.Wait();
            var discoveryUri = new Uri(discoveryTask.Result.Links.User.Href);

            System.Console.WriteLine("Starting authentication");
            try
            {
                var authenticationTask = _authentication.Start(discoveryUri);
                authenticationTask.Wait();
            }
            catch (AggregateException ex)
            {
                if (ex != null && ex.InnerException.GetType() == typeof(AuthenticationException))
                {
                    var challenge = (ex.InnerException as AuthenticationException).Challenge;
                    var authTokenTask = _authentication.GetAuthToken(challenge.MsRtcOAuth, new Credentials
                    {
                        GrantType = Constants.Password,
                        Username = username,
                        Password = password
                    });
                    authTokenTask.Wait();

                    var rootTask = _authentication.Start(discoveryUri, authTokenTask.Result);
                    rootTask.Wait();

                    if (rootTask.Result != null)
                    {
                        System.Console.WriteLine("Creating Application");
                        var applications = new Applications
                        {
                            Culture = "en-US",
                            EndpointId = Guid.NewGuid().ToString(),
                            UserAgent = "tester"
                        };
                        var response = _proxy.ExecuteRequest(new Request
                        {
                            Uri = new Uri(rootTask.Result.Links.Applications.Href),
                            Method = HttpMethod.Post,
                            Headers = new Dictionary<string, string>
                            {
                                { "Content-Type", "application/json" }
                            },
                            Data = applications.ToBytes()
                        });
                        if (response != null)
                        {

                        }
                    }
                }
            }

            System.Console.ReadLine();
        }
    }
}
