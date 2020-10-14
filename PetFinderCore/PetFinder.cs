using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinderCore
{
    public class PetFinder
    {
        private readonly IPetFinderRepositoryClient client;

        public PetFinder(IPetFinderRepositoryClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Person>> GetPeopleAsync()
        {
            return (await client.GetAsync());
        }

        public async Task<IEnumerable<Pet>> GetPetsAsync()
        {
            var people = await client.GetAsync();
            return people.SelectMany(p => p.Pets ?? new Pet[] { });
        }
    }
}