using System;

namespace TrackerApi.Services.Erros
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException() { }

        public AlreadyExistsException(string name)
            : base(name)
        {

        }
    }
}
