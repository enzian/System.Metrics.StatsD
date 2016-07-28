using Xunit;

namespace System.Metrics.StatsD
{
    public class StatsdBackendIntegrationTest
    {
        [Fact(Skip="Non-Isolated test - needs > nc -l 8125 -u")]
        //[Fact]
        public async void TestUdpConnection()
        {
            var subject = new StatsdBackend();
            
            subject.UseUdp("localhost", 8125);

            await subject.Handle("test.metric.total:1|c");
        }

        
        [Fact(Skip="Non-Isolated test - needs > nc -l 8125 -u")]
        //[Fact]
        public async void TestUdpConnection_WithMultipleMetrics()
        {
            var subject = new StatsdBackend();
            
            subject.UseUdp("localhost", 8125);

            await subject.Handle("test.metric.total:1|c");
            await subject.Handle("test.metric.total:1|c");
        }
        
        
        [Fact(Skip="Non-Isolated test - needs > nc -l 8125")]
        //[Fact]
        public async void TestTcpConnection_WithMultipleMetrics()
        {
            var subject = new StatsdBackend();
            
            subject.UseTcp("localhost", 8125);

            await subject.Handle("test.metric.total:1|c");
            await subject.Handle("test.metric.total:1|c");
        }
    }
}