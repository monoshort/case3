using Minor.Miffy;
using RabbitMQ.Client;

namespace BestelService.Seeding.Abstractions
{
    public interface IDatabaseCacher
    {
        void EnsureKlanten(IBusContext<IConnection> context);
    }
}
