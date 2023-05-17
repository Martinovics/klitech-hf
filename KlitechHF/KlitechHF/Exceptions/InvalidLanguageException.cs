using System;




namespace KlitechHF.Exceptions
{
    public class InvalidLanguageException : Exception
    {
        public InvalidLanguageException()
        {
        }

        public InvalidLanguageException(string message) : base(message)
        {
        }

        public InvalidLanguageException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
