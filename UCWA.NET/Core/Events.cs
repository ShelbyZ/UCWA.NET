using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UCWA.NET.Resources;
using UCWA.NET.Transport;

namespace UCWA.NET.Core
{
    public class Events
    {
        public event EventHandler<EventReceivedEventArgs> OnEventReceived;

        public event EventHandler<ErrorEventArgs> OnError;

        public Uri EventsUri { get; private set; }

        private TransportProxy _proxy;

        private Task _eventsLoop;

        private CancellationTokenSource _source;

        private Error _error;

        public Events(TransportProxy proxy, Uri uri)
        {
            _proxy = proxy;
            EventsUri = uri;
        }

        public void Start()
        {
            if (_eventsLoop == null)
            {
                _source = new CancellationTokenSource();
                _eventsLoop = Task.Run(() =>
                {
                    while (!_source.IsCancellationRequested)
                    {
                        EventLoop();
                    }
                }, _source.Token);
            }
        }

        public void Stop()
        {
            if (_eventsLoop != null)
            {
                _source.Cancel();
                _eventsLoop.ContinueWith(task =>
                {
                    _eventsLoop.Dispose();
                    _eventsLoop = null;
                    _source.Dispose();
                    _source = null;

                    if (_error != null)
                    {
                        OnError?.Invoke(this, new ErrorEventArgs
                        {
                            Error = _error
                        });
                        _error = null;
                    }
                });
            }
        }

        private void EventLoop()
        {
            var response = _proxy.ExecuteRequest(new Request
            {
                Uri = EventsUri,
                Method = HttpMethod.Get,
                Timeout = Timeout.Infinite
            });

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var evt = response?.Data?.FromBytes<Event>();
                if (evt != null)
                {
                    EventsUri = new Uri(evt.Links?.Resync?.Href ?? evt.Links.Next.Href, UriKind.Relative);
                    if (evt.Links?.Resync != null)
                    {
                        OnEventReceived?.Invoke(this, new EventReceivedEventArgs
                        {
                            Resource = evt
                        });
                    }
                }
            }
            else if (response.StatusCode == HttpStatusCode.Conflict)
            {
                _error = response?.Data?.FromBytes<Error>();
                Stop();
            }
        }
    }
}
