using System;

namespace FrontendService.Exceptions
{
    [Serializable]
    public class ArtikelDoesNotExistException : Exception
    {
        public long ArtikelId { get; }

        public ArtikelDoesNotExistException(long artikelId) : base($"Artikel {artikelId} does not exist")
        {
            ArtikelId = artikelId;
        }
    }
}
