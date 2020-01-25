using System;

namespace BackOfficeFrontendService.Exceptions
{
    [Serializable]
    public class FunctionalException : Exception
    {
        public FunctionalException(string message) : base(message)
        {
        }
    }
}
