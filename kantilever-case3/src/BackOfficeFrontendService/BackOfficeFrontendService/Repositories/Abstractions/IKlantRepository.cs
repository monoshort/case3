using BackOfficeFrontendService.Models;

namespace BackOfficeFrontendService.Repositories.Abstractions
{
    /// <summary>
    /// Repository to persist klanten
    /// </summary>
    public interface IKlantRepository
    {
        /// <summary>
        /// See if the database is empty or not
        /// </summary>
        bool IsEmpty();

        /// <summary>
        /// Add a klant
        /// </summary>
        void Add(Klant klant);

        /// <summary>
        /// Get a klant by its Id
        /// </summary>
        Klant FindById(long id);
    }
}
