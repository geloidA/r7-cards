namespace Cardmngr.Shared.Utils;

public class BindingAddress
{
    private readonly static Dictionary<string, string> _ipsByHost = new Dictionary<string, string>
    {
        { "localhost", "127.0.0.1" },
        { "+", "0.0.0.0" },
        { "*", "0.0.0.0" }
    };

    private BindingAddress(string host, int port, string scheme)
    {
        Host = host;
        Port = port;
        Scheme = scheme;
    }

    public string Host { get; }
    public int Port { get; }
    public string Scheme { get; }

    public static BindingAddress Parse(string address)
    {
        var hostStart = address.LastIndexOf('/');
        var hostEnd = address.LastIndexOf(':');

        if (hostStart == -1) throw new ArgumentException("Invalid address", nameof(address));

        var host = address.Substring(hostStart + 1, hostEnd - hostStart - 1);
        host = _ipsByHost.GetValueOrDefault(host, host);
        
        var port = int.Parse(address[(hostEnd + 1)..]);
        var scheme = address[..(hostStart - 2)];

        return new BindingAddress(host, port, scheme);
    }
}
