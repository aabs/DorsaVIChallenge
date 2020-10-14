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

        public async Task<IEnumerable<Person>> GetPeopleAsync(int? age = null, Gender? gender = null, string name = null)
        {
            var results = (await client.GetAsync()).AsQueryable();
            if (age.HasValue)
            {
                results = results.Where(p => p.Age == age.Value);
            }
            if (gender.HasValue)
            {
                results = results.Where(p => p.Gender == gender.Value);
            }
            if (!string.IsNullOrWhiteSpace(name))
            {
                results = results.Where(p => p.Name == name);
            }
            return results.ToList();
        }

        public async Task<IEnumerable<Pet>> GetPetsAsync()
        {
            var people = await client.GetAsync();
            return people.SelectMany(p => p.Pets ?? new Pet[] { });
        }
    }
}