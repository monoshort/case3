using BackOfficeFrontendService.DAL;
using Microsoft.EntityFrameworkCore;

namespace BackOfficeFrontendService.Test
{
    internal static class TestHelpers
    {
        /// <summary>
        ///     Inject test data
        /// </summary>
        internal static void InjectData<T>(DbContextOptions<BackOfficeContext> options, params T[] entities)
            where T : class
        {
            using BackOfficeContext context = new BackOfficeContext(options);
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }
    }
}