using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace PetFinderCore
{
    public class PetFinderRepositoryClient : IPetFinderRepositoryClient
    {
        private readonly string endpointUri;

        public PetFinderRepositoryClient(string endpointUri)
        {
            this.endpointUri = endpointUri;
        }

        public async Task<IEnumerable<Person>> GetAsync()
        {
            HttpClient client = new HttpClient();
            try
            {
                string responseBody = await client.GetStringAsync(endpointUri);
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