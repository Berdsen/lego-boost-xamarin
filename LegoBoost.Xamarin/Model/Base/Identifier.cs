namespace LegoBoost.Xamarin.Model.Base
{
    public class Identifier : IIdentifier
    {
        public Identifier(string name, string description, byte referenceByte)
        {
            Name = name;
            Description = description;
            ReferenceByte = referenceByte;
        }

        public string Name { get; }

        public string Description { get; }

        public byte ReferenceByte { get; }
    }
}