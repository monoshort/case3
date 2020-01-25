using FrontendService.DAL;
using Microsoft.EntityFrameworkCore;

namespace FrontendService.Test
{
    internal static class TestHelpers
    {
        /// <summary>
        /// Inject test data
        /// </summary>
        internal static void InjectData<T>(DbContextOptions<FrontendContext> options, params T[] entities)
            where T : class
        {
            using FrontendContext context = new FrontendContext(options);
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }
    }
}
