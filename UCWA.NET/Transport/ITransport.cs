namespace UCWA.NET.Transport
{
    public interface ITransport
    {
        Response ExecuteRequest(Request request);
    }
}
