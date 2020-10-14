using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetFinderCore
{
    public class PetFinderRepositoryClient : IPetFinderRepositoryClient
    {
        private readonly IEndpointProvider endpointProvider;

        public PetFinderRepositoryClient(IEndpointProvider endpointProvider)
        {
            this.endpointProvider = endpointProvider;
        }

        public string EndpointName { get; set; }

        public async Task<IEnumerable<Person>> GetAsync(string fromLocation)
        {
            HttpClient client = new HttpClient();
            try
            {
                string responseBody = await client.GetStringAsync(endpointProvider.Endpoints[fromLocation]);
                List<Person> result = JsonConvert.DeserializeObject<List<Person>>(responseBody);
                return result;
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException Caught!");
                Console.WriteLine("Message :{0} ", e.Message);
            }
            return new Person[] { };
        }
    }
}