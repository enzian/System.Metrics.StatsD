namespace System.Metrics.StatsD
{
    public static class EndpointExtensions
    {
        public static IMetricsEndpoint AddStatsD(this IMetricsEndpoint endpoint)
        {
            endpoint.AddBackend(new StatsdBackend());
            return endpoint;
        }

        public static IMetricsEndpoint AddStatsD(this IMetricsEndpoint endpoint, Action<StatsdBackend> setup)
        {
            var backend = new StatsdBackend();

            setup(backend);

            endpoint.AddBackend(backend);
            return endpoint;
        }
    }
}