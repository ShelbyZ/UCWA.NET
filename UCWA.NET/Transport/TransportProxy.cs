namespace UCWA.NET.Transport
{
    public class TransportProxy
    {
        private ITransport _transport;

        public TransportProxy(ITransport transport)
        {
            _transport = transport;
        }

        public Response ExecuteRequest(Request request)
        {
            return _transport.ExecuteRequest(request);
        }
    }
}
