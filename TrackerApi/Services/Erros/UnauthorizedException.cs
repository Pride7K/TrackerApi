using System;

namespace TrackerApi.Services.Erros
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException() { }

        public UnauthorizedException(string name)
            : base(name)
        {

        }
    }
}
