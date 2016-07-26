using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace System.Metrics.StatsD
{
    public static class StatsdBackendExtensions
    {
        public static StatsdBackend UseUdp(this StatsdBackend backend, string host = "localhost", int port = 8125, int? mtuSize = null)
        {
            var sending_socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            // Do the name resolution on the host string
            IPAddress ip;
            if(!IPAddress.TryParse(host, out ip))
            {
                var resolutionTask = Dns.GetHostAddressesAsync(host);
                resolutionTask.Wait();
                
                var addresses = resolutionTask.Result;

                if(addresses.Any(x => x.AddressFamily == AddressFamily.InterNetworkV6))
                {
                    ip = addresses.First(x => x.AddressFamily == AddressFamily.InterNetworkV6);
                }
                else if(addresses.Any(x => x.AddressFamily == AddressFamily.InterNetwork))
                {
                    ip = addresses.First(x => x.AddressFamily == AddressFamily.InterNetwork);                    
                }
                else
                {
                    throw new Exception("Cannot resolve the given host to an IPv4 or IPv6 address!");
                }
            }

            var endpoint = new IPEndPoint(ip, port);

            backend.Socket = sending_socket;

            return backend;
        }
    }
}