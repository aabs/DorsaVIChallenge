namespace PetFinderCore
{
    public enum PetKind
    {
        Cat, Dog, Fish
    }

    public class Pet
    {
        public string Name { get; set; }
        public PetKind Type { get; set; }
    }
}