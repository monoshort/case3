using System;
using BestelService.Core.Models;

namespace BestelService.Core.Exceptions
{
    [Serializable]
    public class BedragKanNietWordenAfgeboektWordenException : Exception
    {
        public Bestelling Bestelling { get; }

        public BedragKanNietWordenAfgeboektWordenException(Bestelling bestelling, string message) : base(message)
        {
            Bestelling = bestelling;
        }
    }
}
