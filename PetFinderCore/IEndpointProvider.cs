using System.Collections.Generic;

namespace PetFinderCore
{
    public interface IEndpointProvider
    {
        IDictionary<string, string> Endpoints { get; }

        void AddEndpoint(string name, string url);
    }
}