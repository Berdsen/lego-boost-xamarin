namespace LegoBoost.Core.Model.Constants
{
    public class HubActions
    {
        public const byte Command = 0x02;

        public class ActionBytes
        {
            // DOWNSTREAM 0x00 - 0x2F
            public const byte SwitchHubOff = 0x01;
            public const byte Disconnect = 0x02;
            public const byte VccPortControlOn = 0x03;
            public const byte VccPortControlOff = 0x04;
            public const byte ActivateBusy = 0x05;
            public const byte DeactivateBusy = 0x06;
            public const byte ShutdownImmediately = 0x2F;

            // UPSTREAM 0x30 - 0x64
            public const byte HubWillSwitchOff = 0x30;
            public const byte HubWillDisconnect = 0x31;
            public const byte HubWillGoIntoBootMode = 0x32;
        }

        public class ActionNames
        {
            public const string SwitchHubOff = "SwitchHubOff";
            public const string Disconnect = "Disconnect";
            public const string VccPortControlOn = "VccPortControlOn";
            public const string VccPortControlOff = "VccPortControlOff";
            public const string ActivateBusy = "ActivateBusy";
            public const string DeactivateBusy = "DeactivateBusy";
            public const string ShutdownImmediately = "ShutdownImmediately";
            public const string HubWillSwitchOff = "HubWillSwitchOff";
            public const string HubWillDisconnect = "HubWillDisconnect";
            public const string HubWillGoIntoBootMode = "HubWillGoIntoBootMode";
        }

    }
}