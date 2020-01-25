using BestelService.Infrastructure.DAL;
using Microsoft.EntityFrameworkCore;

namespace BestelService.Test
{
    internal static class TestHelpers
    {
        /// <summary>
        /// Inject test data
        /// </summary>
        internal static void InjectData<T>(DbContextOptions<BestelContext> options, params T[] entities)
            where T : class
        {
            using BestelContext context = new BestelContext(options);
            context.Set<T>().AddRange(entities);
            context.SaveChanges();
        }
    }
}
