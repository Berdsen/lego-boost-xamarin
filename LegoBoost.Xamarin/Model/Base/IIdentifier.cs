namespace LegoBoost.Xamarin.Model.Base
{
    public interface IIdentifier
    {
        string Name { get; }

        string Description { get; }

        byte ReferenceByte { get; }
    }
}