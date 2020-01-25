using Minor.Miffy;
using RabbitMQ.Client;

namespace BackOfficeFrontendService.Seeding.Abstractions
{
    public interface IDatabaseCacher
    {
        void EnsureVoorraad();
        void EnsureKlanten(IBusContext<IConnection> context);
        void EnsureBestellingen(IBusContext<IConnection> context);
    }
}
