using System.Diagnostics.CodeAnalysis;

namespace BestelService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class EnvNames
    {
        internal const string DbConnectionString = "DB_CONNECTION_STRING";
        internal const string ReplayExchangeName = "BROKER_REPLAY_EXCHANGE_NAME";
        internal const string AuditLoggerUrl = "AUDITLOGGER_URL";
    }
}
