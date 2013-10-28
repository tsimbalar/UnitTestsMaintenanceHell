using System;

namespace ProductionCode.Lib.Data
{
    public class AccountNotfFoundException : Exception
    {
        public AccountNotfFoundException()
        {
        }

        public AccountNotfFoundException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}