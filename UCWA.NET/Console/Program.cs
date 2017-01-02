using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
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
        static Timer _timer;

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
                if (ex?.InnerException.GetType() == typeof(AuthenticationException))
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
                        var applications = new Application
                        {
                            Culture = "en-US",
                            EndpointId = Guid.NewGuid().ToString(),
                            UserAgent = "tester"
                        };
                        var appTask = _proxy.ExecuteRequestAsync(new Request
                        {
                            Uri = new Uri(rootTask.Result.Links.Applications.Href),
                            Method = HttpMethod.Post,
                            Headers = new Dictionary<string, string>
                            {
                                { Constants.ContentType, Constants.Json }
                            },
                            Data = applications.ToBytes()
                        });
                        appTask.Wait();

                        if (appTask?.Result?.StatusCode == HttpStatusCode.Created)
                        {
                            applications = appTask.Result.Data.FromBytes<Application>();
                            if (applications != null)
                            {
                                var makeMeAvailable = new MakeMeAvailable
                                {
                                    SupportedModalities = new [] { "Messaging" }
                                };
                                var makeMeAvailableTask = _proxy.ExecuteRequestAsync(new Request
                                {
                                    Uri = new Uri(applications.Embedded.Me.Links.MakeMeAvailable.Href, UriKind.Relative),
                                    Method = HttpMethod.Post,
                                    Headers = new Dictionary<string, string>
                                    {
                                        { Constants.ContentType, Constants.Json }
                                    },
                                    Data = makeMeAvailable.ToBytes()
                                });
                                makeMeAvailableTask.Wait();

                                if (makeMeAvailableTask?.Result.StatusCode == HttpStatusCode.NoContent)
                                {
                                    // Start events loop
                                    var events = new Events(_proxy, new Uri(applications.Links.Events.Href, UriKind.Relative));
                                    events.OnEventReceived += OnEventReceived;
                                    events.Start();

                                    // Request and start reportMyActivity loop
                                    var meTask = _proxy.ExecuteRequestAsync(new Request
                                    {
                                        Uri = new Uri(applications.Embedded.Me.Links.Self.Href, UriKind.Relative),
                                        Method = HttpMethod.Get
                                    });
                                    meTask.Wait();

                                    if (meTask?.Result?.StatusCode == HttpStatusCode.OK)
                                    {
                                        var me = meTask.Result.Data.FromBytes<Me>();
                                        _timer = new Timer(ReportMyActivity, me.Links.ReportMyActivity.Href, 0, (int)(3.5 * 60 * 1000));
                                    }

                                    // Request and update communication
                                    var commGetTask = _proxy.ExecuteRequestAsync(new Request
                                    {
                                        Uri = new Uri(applications.Embedded.Communication.Links.Self.Href, UriKind.Relative),
                                        Method = HttpMethod.Get
                                    });
                                    commGetTask.Wait();

                                    if (commGetTask?.Result?.StatusCode == HttpStatusCode.OK)
                                    {
                                        var comm = commGetTask.Result.Data.FromBytes<Communication>();
                                        comm.SupportedMessageFormats.Add("Html");
                                        var commTask = _proxy.ExecuteRequestAsync(new Request
                                        {
                                            Uri = new Uri(applications.Embedded.Communication.Links.Self.Href, UriKind.Relative),
                                            Method = HttpMethod.Put,
                                            Headers = new Dictionary<string, string>
                                            {
                                                { Constants.ContentType, Constants.Json },
                                                { Constants.IfMatch, string.Format("\"{0}\"", comm.ETag) }
                                            },
                                            Data = comm.ToBytes()
                                        });
                                        commTask.Wait();

                                        if (commTask?.Result?.StatusCode == HttpStatusCode.NoContent)
                                        {
                                            System.Console.WriteLine("Communication Updated");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            System.Console.ReadLine();
            _timer.Dispose();
            _timer = null;
        }

        static void OnEventReceived(object sender, EventReceivedEventArgs e)
        {
            if (e.Resource != null)
            {
                System.Console.WriteLine("Received Event {0}", (e.Resource as Event).Links.Self.Href);
            }
        }

        static void ReportMyActivity(object o)
        {
            var task = _proxy.ExecuteRequestAsync(new Request
            {
                Uri = new Uri(o as string, UriKind.Relative),
                Method = HttpMethod.Post
            });
            task.Wait();
        }
    }
}
