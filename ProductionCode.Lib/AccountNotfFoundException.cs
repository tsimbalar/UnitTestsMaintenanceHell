﻿using System;

namespace ProductionCode.Lib
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