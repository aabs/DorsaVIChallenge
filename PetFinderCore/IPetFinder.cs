using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetFinderCore
{
    public interface IPetFinder
    {
        Task<IEnumerable<Person>> GetPeopleAsync(string fromLocation,
            int? age = null,
            Gender? gender = null,
            string name = null,
            PetKind? kind = null);

        Task<IEnumerable<Pet>> GetPetsAsync(string fromLocation);
    }
}