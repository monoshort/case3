using KlantService.DAL;
using Microsoft.EntityFrameworkCore;

namespace KlantService.Test
{
    internal static class TestHelpers
    {
        /// <summary>
        /// Inject test data
        /// </summary>
        internal static void InjectData<T>(DbContextOptions<KlantContext> options, params T[] entities)
            where T : class
        {
            using KlantContext context = new KlantContext(options);
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }
    }
}
