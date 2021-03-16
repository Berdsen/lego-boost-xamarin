namespace LegoBoost.Core.Model.Constants
{
    public class HubProperties
    {
        public const byte Command = 0x01;

        public class PropertyBytes
        {
            public const byte AdvertisingName = 0x01;
            public const byte Button = 0x02;
            public const byte FirmwareVersion = 0x03;
            public const byte HardwareVersion = 0x04;
            public const byte Rssi = 0x05;
            public const byte BatteryVoltage = 0x06;
            public const byte BatteryType = 0x07;
            public const byte ManufacturerName = 0x08;
            public const byte RadioFirmwareVersion = 0x09;
            public const byte LWPProtocolVersion = 0x0A;
            public const byte SystemTypeId = 0x0B;
            public const byte HardwareNetworkId = 0x0C;
            public const byte PrimaryMacAdress = 0x0D;
            public const byte SecondaryMacAdress = 0x0E;
            public const byte HardwareNetworkFamily = 0x0F;
        }

        public class PropertyNames
        {
            public const string AdvertisingName = "AdvertisingName";
            public const string Button = "Button";
            public const string FirmwareVersion = "FirmwareVersion";
            public const string HardwareVersion = "HardwareVersion";
            public const string Rssi = "Rssi";
            public const string BatteryVoltage = "BatteryVoltage";
            public const string BatteryType = "BatteryType";
            public const string ManufacturerName = "ManufacturerName";
            public const string RadioFirmwareVersion = "RadioFirmwareVersion";
            public const string LWPProtocolVersion = "LWPProtocolVersion";
            public const string SystemTypeId = "SystemTypeId";
            public const string HardwareNetworkId = "HardwareNetworkId";
            public const string PrimaryMacAdress = "PrimaryMacAdress";
            public const string SecondaryMacAdress = "SecondaryMacAdress";
            public const string HardwareNetworkFamily = "HardwareNetworkFamily";
        }

        public class PropertyOperations
        {
            public const byte Set = 0x01;
            public const byte EnableUpdates = 0x02;
            public const byte DisableUpdates = 0x03;
            public const byte Reset = 0x04;
            public const byte RequestUpdate = 0x05;
            public const byte Update = 0x06;
        }
    }
}