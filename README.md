# System.Metrics.StatsD

This is an extension library for the `System.Metrics` library. A StatsD backend can be added as follows:

```csharp
using System.Metrics;
using System.Metrisc.StatsD;

namespace TestApp
{
    public class Program
    {
        public void main(string[] args)
        {
            // Initialize a standard metrics endpoint
            var subject = new StandardEndpoint();
            subject.AddStatsD(setup => {
                // Use a UDP backend sending metrics to localhost port 8125
                setup.UseUdp();

                // This can be writte more verbose like this if needed:
                setup.UseUdp("localhost", 8125);
            })

            subject.Record<Counting>("metric.test.total", 1)
        }
    }
}
```