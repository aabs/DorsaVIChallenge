using System.Collections;
using System.Collections.Generic;

namespace PetFinderWeb.Models
{
    public class Person
    {
        public uint Age { get; set; }
        public PetFinderCore.Gender Gender { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public IEnumerable<Pet> Pets { get; set; }
    }

    public class Pet
    {
        public string Name { get; set; }
        public PetFinderCore.PetKind Type { get; set; }
    }
}