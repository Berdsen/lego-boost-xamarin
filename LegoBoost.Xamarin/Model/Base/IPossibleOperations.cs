namespace LegoBoost.Xamarin.Model.Base
{
    public interface IPossibleOperations
    {
        bool CanSet { get; set; }

        bool CanEnableUpdate { get; set; }

        bool CanDisableUpdate { get; set; }

        bool CanReset { get; set; }

        bool CanRequestUpdate { get; set; }

        bool CanUpdate { get; set; }
    }
}