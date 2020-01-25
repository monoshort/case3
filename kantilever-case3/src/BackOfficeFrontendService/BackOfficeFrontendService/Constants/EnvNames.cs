using System.Diagnostics.CodeAnalysis;

namespace BackOfficeFrontendService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class EnvNames
    {
        internal const string DbConnectionString = "DB_CONNECTION_STRING";
        internal const string AuthClientSecret = "AUTH_CLIENT_SECRET";
        internal const string AuthClientId = "AUTH_CLIENT_ID";
        internal const string AuthenticationServerAddress = "AUTH_AUTHORITY";
        internal const string VoorraadServiceUrl = "VOORRAAD_SERVICE_URL";
        internal const string CatalogusServiceUrl = "CATALOGUS_SERVICE_URL";
        internal const string ReplayExchangeName = "BROKER_REPLAY_EXCHANGE_NAME";
        internal const string AuditLoggerUrl = "AUDITLOGGER_URL";
    }
}
