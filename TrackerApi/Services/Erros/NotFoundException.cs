using System;

namespace TrackerApi.Services.Erros
{
    public class NotFoundException : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string name)
            : base( name)
        {

        }
    }
}
