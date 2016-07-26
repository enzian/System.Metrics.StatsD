using Xunit;

namespace System.Metrics.StatsD
{
    public class StatsdBackendIntegrationTest
    {
        [Fact(Skip="Non-Isolated test - needs > nc -i 8125 -u")]
        //[Fact]
        public async void TestUdpConnection()
        {
            var subject = new StatsdBackend();
            
            subject.UseUdp("localhost", 8125);

            await subject.Handle("test.metric.total:1|c");
        }
    }
}