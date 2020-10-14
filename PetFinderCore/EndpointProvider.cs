using System;
using System.Collections;
using System.Collections.Generic;

namespace PetFinderCore
{
    public class EndpointProvider : IEndpointProvider
    {
        public EndpointProvider()
        {
            Endpoints = new Dictionary<string, string>();
        }

        public IDictionary<string, string> Endpoints { get; private set; }

        public void AddEndpoint(string name, string url)
        {
            Endpoints[name] = url;
        }
    }
}