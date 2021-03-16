using LegoBoost.Xamarin.Model.Base;

namespace LegoBoost.Xamarin.Model
{
    public sealed class HubPropertyConfig : Identifier, IPossibleOperations
    {
        public HubPropertyConfig(string name, string description, byte referenceByte) : base(name, description, referenceByte)
        {
        }

        public bool CanSet { get; set; } = false;
        public bool CanEnableUpdate { get; set; } = false;
        public bool CanDisableUpdate { get; set; } = false;
        public bool CanReset { get; set; } = false;
        public bool CanRequestUpdate { get; set; } = true;
        public bool CanUpdate { get; set; } = true;
    }
}