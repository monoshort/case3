using Minor.Miffy;
using RabbitMQ.Client;

namespace FrontendService.Seeding.Abstractions
{
    public interface IDatabaseCacher
    {
        void EnsureArtikelen();
        void EnsureKlanten(IBusContext<IConnection> context);
        void EnsureBestellingen(IBusContext<IConnection> context);
    }
}
