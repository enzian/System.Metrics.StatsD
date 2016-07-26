using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace System.Metrics.StatsD
{
    public class StatsdBackend : IMetricsSink
    {
        public Socket Socket { get; internal set; }
        
        public Task Handle(string metricRecord)
        {
            return Task.Run(
                () => 
                {
                    Socket.SendTo(Encoding.UTF8.GetBytes(metricRecord + "\n"), new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8125));
                });
        }
    }
}