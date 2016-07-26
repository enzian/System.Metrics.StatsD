namespace System.Metrics.StatsD
{
    public static class EndpointExtensions
    {
        public static Endpoint AddStatsD(this Endpoint endpoint)
        {
            endpoint.AddBackend(new StatsdBackend());
            return endpoint;
        }

        public static Endpoint AddStatsD(this Endpoint endpoint, Action<StatsdBackend> setup)
        {
            var backend = new StatsdBackend();

            setup(backend);

            endpoint.AddBackend(backend);
            return endpoint;
        }
    }
}