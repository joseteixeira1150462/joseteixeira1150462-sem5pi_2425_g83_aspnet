using System;

namespace HealthCare.Domain.Shared
{
    public class ResourceUnavailableException : Exception
    {
        public string Details { get; }

        public ResourceUnavailableException(string message) : base(message)
        {
            
        }

        public ResourceUnavailableException(string message, string details) : base(message)
        {
            this.Details = details;
        }
    }
}