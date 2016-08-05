using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace System.Metrics.StatsD
{
    public static class StatsdBackendExtensions
    {
        public static StatsdBackend UseUdp(this StatsdBackend backend, string host = "localhost", int port = 8125, int? mtuSize = null)
        {
            var ip = Resolve(host, port);
            var endpoint = new IPEndPoint(ip, port);
            var udpClient = new UdpClient();

            backend.Send = delegate (byte[] content) {
                return udpClient.SendAsync(content, content.Length, endpoint)
                    .ContinueWith(x => Console.WriteLine($"Sent: {x.Result} bytes"));
            };

            return backend;
        }
        
        public static StatsdBackend UseTcp(this StatsdBackend backend, string host = "localhost", int port = 8125, int? mtuSize = null)
        {
            var ip = Resolve(host, port);
            var tcpClient = new TcpClient();
            NetworkStream stream;
            
            try
            {
                 tcpClient.ConnectAsync(ip, port).Wait();
                 stream = tcpClient.GetStream();
            }
            catch (System.Exception)
            {
                throw new Exception($"Cannot cannot connect via TCP to {ip}:{port} !");
            }

            // Object used for synchronization
            var obj = new object();

            backend.Send = delegate (byte[] content) {
                return Task.Run(
                    () => 
                    {
                        lock (obj)
                        {
                            try
                            {
                                stream.Write(content, 0, content.Length);
                                stream.Flush();
                            }
                            catch (System.Exception)
                            {
                                // Of there is a problem with the connection - open a new one.
                                // IDEA: implement retries with exponential back-off time here!
                                tcpClient = new TcpClient();
                                tcpClient.ConnectAsync(ip, port).Wait();
                                stream = tcpClient.GetStream();
                            }
                        }
                    });
            };

            return backend;
        }

        private static IPAddress Resolve(string host, int port)
        {
            IPAddress ip;

            // Do the name resolution on the host string
            if(!IPAddress.TryParse(host, out ip))
            {
                var resolutionTask = Dns.GetHostAddressesAsync(host);
                resolutionTask.Wait();
                
                var addresses = resolutionTask.Result;

                if(addresses.Any(x => x.AddressFamily == AddressFamily.InterNetwork))
                {
                    ip = addresses.First(x => x.AddressFamily == AddressFamily.InterNetwork);
                }
                else if(addresses.Any(x => x.AddressFamily == AddressFamily.InterNetworkV6))
                {
                    ip = addresses.First(x => x.AddressFamily == AddressFamily.InterNetworkV6);                    
                }
                else
                {
                    throw new Exception("Cannot resolve the given host to an IPv4 or IPv6 address!");
                }
            }

            return ip;
        }
    }
}