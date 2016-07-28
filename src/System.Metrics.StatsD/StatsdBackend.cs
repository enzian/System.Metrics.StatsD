using System.Threading.Tasks;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace System.Metrics.StatsD
{
    public class StatsdBackend : IMetricsSink
    {
        internal StatsSender Send { get; set; }
        
        public async Task Handle(string metricRecord)
        {
            await Send(Encoding.UTF8.GetBytes(metricRecord + "\n"));
        }
    }
}