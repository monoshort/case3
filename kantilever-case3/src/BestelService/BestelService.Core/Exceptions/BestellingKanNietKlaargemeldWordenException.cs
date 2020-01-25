using System;
using BestelService.Core.Models;

namespace BestelService.Core.Exceptions
{
    [Serializable]
    public class BestellingKanNietKlaargemeldWordenException : Exception
    {
        public Bestelling Bestelling { get; }

        public BestellingKanNietKlaargemeldWordenException(Bestelling bestelling, string message) : base(message)
        {
            Bestelling = bestelling;
        }
    }
}
