using LegoBoost.Core.Utilities;

namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public static partial class Hub
    {
        public static class Action
        {
            public enum Name : byte
            {
                // DOWNSTREAM 0x00 - 0x2F
                [StringValue(nameof(SwitchHubOff))]
                SwitchHubOff = 0x01,
                [StringValue(nameof(Disconnect))]
                Disconnect = 0x02,
                [StringValue(nameof(VccPortControlOn))]
                VccPortControlOn = 0x03,
                [StringValue(nameof(VccPortControlOff))]
                VccPortControlOff = 0x04,
                [StringValue(nameof(ActivateBusy))]
                ActivateBusy = 0x05,
                [StringValue(nameof(DeactivateBusy))]
                DeactivateBusy = 0x06,
                [StringValue(nameof(ShutdownImmediately))]
                ShutdownImmediately = 0x2F,

                // UPSTREAM 0x30 - 0x64
                [StringValue(nameof(HubWillSwitchOff))]
                HubWillSwitchOff = 0x30,
                [StringValue(nameof(HubWillDisconnect))]
                HubWillDisconnect = 0x31,
                [StringValue(nameof(HubWillGoIntoBootMode))]
                HubWillGoIntoBootMode = 0x32
            }
        }

    }
}