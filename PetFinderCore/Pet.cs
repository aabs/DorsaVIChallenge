namespace PetFinderCore
{
    public enum PetKind
    {
        Cat, Dog, Fish, Bird
    }

    public class Pet
    {
        public string Name { get; set; }
        public PetKind Type { get; set; }
    }
}