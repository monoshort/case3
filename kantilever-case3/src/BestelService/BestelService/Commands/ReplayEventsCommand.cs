using System.Diagnostics.CodeAnalysis;

namespace BestelService.Commands
{
    [ExcludeFromCodeCoverage]
    public class ReplayEventsCommand
    {
        public string ExchangeName { get; set; }
        public long? ToTimestamp { get; set; }
        public string EventType { get; set; }
        public string TopicFilter { get; set; }
    }
}
