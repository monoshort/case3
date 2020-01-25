using System.Diagnostics.CodeAnalysis;

namespace FrontendService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class EnvNames
    {
        internal const string DbConnectionString = "DB_CONNECTION_STRING";
        internal const string CatalogusServiceUrl = "CATALOGUS_SERVICE_URL";
        internal const string VoorraadServiceUrl = "VOORRAAD_SERVICE_URL";
        internal const string AuthenticationServerAddress = "AUTH_AUTHORITY";
        internal const string ReplayExchangeName = "BROKER_REPLAY_EXCHANGE_NAME";
        internal const string AuditLoggerUrl = "AUDITLOGGER_URL";

        internal const string AngularAuthority = "ANGULAR_AUTHORITY";
        internal const string AngularClientId = "ANGULAR_CLIENTID";
        internal const string AngularReponseType = "ANGULAR_RESPONSE_TYPE";
        internal const string AngularRedirectUri = "ANGULAR_REDIRECT_URI";
        internal const string AngularScope = "ANGULAR_SCOPE";
        internal const string AngularPostLogoutRedirectUri = "ANGULAR_POST_LOGOUT_REDIRECT_URI";
    }
}
