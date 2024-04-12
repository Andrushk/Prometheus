using System.Diagnostics.Metrics;

namespace WebApi
{
    public class GreetingMetrics
    {
        private readonly Counter<int> _greetingCounter;

        public GreetingMetrics(IMeterFactory meterFactory)
        {
            var meter = meterFactory.Create("My.Greetings");
            _greetingCounter = meter.CreateCounter<int>("user.greetings");
        }

        public void Greet(string name, int quantity = 1)
        {
            _greetingCounter.Add(quantity, new KeyValuePair<string, object?>("user.name", name));
        }
    }
}
