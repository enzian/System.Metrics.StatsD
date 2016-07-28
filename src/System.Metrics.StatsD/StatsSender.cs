using System.Threading.Tasks;

namespace System.Metrics.StatsD
{
    internal delegate Task StatsSender(byte[] content);
}