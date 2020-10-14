using System.Collections.Generic;

namespace PetFinderCore
{
    public class Person
    {
        public uint Age { get; set; }
        public Gender Gender { get; set; }
        public string Name { get; set; }

        public IEnumerable<Pet> Pets { get; set; }
    }
}