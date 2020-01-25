using System.Diagnostics.CodeAnalysis;

namespace IdentityService.Constants
{
    [ExcludeFromCodeCoverage]
    internal static class EnvNames
    {
        internal const string DbConnectionString = "DB_CONNECTION_STRING";
        internal const string ClientsPath = "CONFIG_PATH_CLIENTS";
        internal const string IdsPath = "CONFIG_PATH_IDS";
        internal const string ApisPath = "CONFIG_PATH_APIS";
    }
}
