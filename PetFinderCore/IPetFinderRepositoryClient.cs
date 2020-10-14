using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetFinderCore
{
    public interface IPetFinderRepositoryClient
    {
        Task<IEnumerable<Person>> GetAsync();
    }
}