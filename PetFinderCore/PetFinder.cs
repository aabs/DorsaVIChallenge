using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PetFinderCore
{
    public class PetFinder : IPetFinder
    {
        private readonly IPetFinderRepositoryClient client;

        public PetFinder(IPetFinderRepositoryClient client)
        {
            this.client = client;
        }

        public async Task<IEnumerable<Person>> GetPeopleAsync(string fromLocation,
            int? age = null,
            Gender? gender = null,
            string name = null,
            PetKind? kind = null)
        {
            var results = (await client.GetAsync(fromLocation));
            if (age.HasValue)
            {
                results = results.Where(p => p.Age == age.Value);
            }
            if (gender.HasValue)
            {
                results = results.Where(p => p.Gender == gender.Value);
            }
            if (!string.IsNullOrWhiteSpace(name) && name != "ignore")
            {
                results = results.Where(p => p.Name == name);
            }

            foreach (var r in results)
            {
                r.Location = fromLocation;
                r.Pets ??= (new Pet[] { });
            }

            if (kind.HasValue)
            { // filter out people who dont have the pet required
                foreach (var p in results)
                {
                    var tmp = p.Pets.Where(x => x.Type == kind.Value);
                    p.Pets = tmp.ToList();
                }
            }
            var blah = results.Where(p => p.Pets.Count() > 0).ToList();
            return blah;
        }

        public async Task<IEnumerable<Pet>> GetPetsAsync(string fromLocation)
        {
            var people = await client.GetAsync(fromLocation);
            return people.SelectMany(p => p.Pets ?? new Pet[] { });
        }
    }
}