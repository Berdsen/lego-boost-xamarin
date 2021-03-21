namespace LegoBoost.Core.Model
{
    public interface IVirtualDevice : IAttachedIO
    {
        byte PortA { get; }
        
        byte PortB { get; }

    }
}