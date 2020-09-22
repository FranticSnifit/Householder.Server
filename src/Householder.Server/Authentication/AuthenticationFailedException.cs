using System;

namespace Householder.Server.Authentication
{
    public class AuthenticationFailedException : Exception
    {
        public AuthenticationFailedException(string message) : base(message)
        {
            
        }
    }
}