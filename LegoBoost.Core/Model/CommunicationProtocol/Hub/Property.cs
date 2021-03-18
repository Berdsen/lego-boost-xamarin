using LegoBoost.Core.Utilities;

namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public static partial class Hub
    {
        public static class Property
        {
            public const byte Command = 0x01;

            public enum Name : byte
            {
                [StringValue(nameof(AdvertisingName))]
                AdvertisingName = 0x01,
                [StringValue(nameof(Button))]
                Button = 0x02,
                [StringValue(nameof(FirmwareVersion))]
                FirmwareVersion = 0x03,
                [StringValue(nameof(HardwareVersion))]
                HardwareVersion = 0x04,
                [StringValue(nameof(Rssi))]
                Rssi = 0x05,
                [StringValue(nameof(BatteryVoltage))]
                BatteryVoltage = 0x06,
                [StringValue(nameof(BatteryType))]
                BatteryType = 0x07,
                [StringValue(nameof(ManufacturerName))]
                ManufacturerName = 0x08,
                [StringValue(nameof(RadioFirmwareVersion))]
                RadioFirmwareVersion = 0x09,
                [StringValue(nameof(LWPProtocolVersion))]
                LWPProtocolVersion = 0x0A,
                [StringValue(nameof(SystemTypeId))]
                SystemTypeId = 0x0B,
                [StringValue(nameof(HardwareNetworkId))]
                HardwareNetworkId = 0x0C,
                [StringValue(nameof(PrimaryMacAdress))]
                PrimaryMacAdress = 0x0D,
                [StringValue(nameof(SecondaryMacAdress))]
                SecondaryMacAdress = 0x0E,
                [StringValue(nameof(HardwareNetworkFamily))]
                HardwareNetworkFamily = 0x0F
            }

            public enum Operation : byte
            {
                Set = 0x01,
                EnableUpdates = 0x02,
                DisableUpdates = 0x03,
                Reset = 0x04,
                RequestUpdate = 0x05,
                Update = 0x06
            }
        }
    }
}