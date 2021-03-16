using LegoBoost.Core.Model.Constants;
using LegoBoost.Core.Model.Responses;
using LegoBoost.Core.Utilities;
using LegoBoost.Xamarin.Utilities;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LegoBoost.Xamarin.Model
{
    public class Hub : IDisposable
    {
        private ICharacteristic hubCharacteristic;
        private bool waitingForUpdate = false;

        public Dictionary<string, HubProperty> Properties { get; }

        public Hub(ICharacteristic hubCharacteristic)
        {
            this.hubCharacteristic = hubCharacteristic;
            this.hubCharacteristic.ValueUpdated += ValueUpdated;

            Properties = new Dictionary<string, HubProperty>
            {
                {HubProperties.PropertyNames.AdvertisingName, new HubProperty(new HubPropertyConfig("Adv. Name", "", HubProperties.PropertyBytes.AdvertisingName) {CanSet = true, CanEnableUpdate = true, CanDisableUpdate = true, CanReset = true})},
                {HubProperties.PropertyNames.Button, new HubProperty(new HubPropertyConfig("Button", "", HubProperties.PropertyBytes.Button) {CanEnableUpdate = true, CanDisableUpdate = true})},
                {HubProperties.PropertyNames.FirmwareVersion, new HubProperty(new HubPropertyConfig("FW Version", "", HubProperties.PropertyBytes.FirmwareVersion))},
                {HubProperties.PropertyNames.HardwareVersion, new HubProperty(new HubPropertyConfig("HW Version", "", HubProperties.PropertyBytes.HardwareVersion))},
                {HubProperties.PropertyNames.Rssi, new HubProperty(new HubPropertyConfig("RSSI", "", HubProperties.PropertyBytes.Rssi) {CanEnableUpdate = true, CanDisableUpdate = true})},
                {HubProperties.PropertyNames.BatteryVoltage, new HubProperty(new HubPropertyConfig("Battery Voltage", "", HubProperties.PropertyBytes.BatteryVoltage) {CanEnableUpdate = true, CanDisableUpdate = true})},
                {HubProperties.PropertyNames.BatteryType, new HubProperty(new HubPropertyConfig("Battery Type", "", HubProperties.PropertyBytes.BatteryType))},
                {HubProperties.PropertyNames.ManufacturerName, new HubProperty(new HubPropertyConfig("Manufacturer Name", "", HubProperties.PropertyBytes.ManufacturerName))},
                {HubProperties.PropertyNames.RadioFirmwareVersion, new HubProperty(new HubPropertyConfig("Radio Firmware Version", "", HubProperties.PropertyBytes.RadioFirmwareVersion))},
                {HubProperties.PropertyNames.LWPProtocolVersion, new HubProperty(new HubPropertyConfig("LWP Protocol Version", "", HubProperties.PropertyBytes.LWPProtocolVersion))},
                {HubProperties.PropertyNames.SystemTypeId, new HubProperty(new HubPropertyConfig("System Type ID", "", HubProperties.PropertyBytes.SystemTypeId))},
                {HubProperties.PropertyNames.HardwareNetworkId, new HubProperty(new HubPropertyConfig("H/W NetWork ID", "", HubProperties.PropertyBytes.HardwareNetworkId) {CanSet = true, CanReset = true})},
                {HubProperties.PropertyNames.PrimaryMacAdress, new HubProperty(new HubPropertyConfig("Primary MAC adress", "", HubProperties.PropertyBytes.PrimaryMacAdress))},
                {HubProperties.PropertyNames.SecondaryMacAdress, new HubProperty(new HubPropertyConfig("Secondary MAC adress", "", HubProperties.PropertyBytes.SecondaryMacAdress))},
                {HubProperties.PropertyNames.HardwareNetworkFamily, new HubProperty(new HubPropertyConfig("H/W Network Family", "", HubProperties.PropertyBytes.HardwareNetworkFamily) {CanSet = true})}
            };
        }

        private void ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            if (waitingForUpdate) return;

            System.Diagnostics.Debug.WriteLine($"Name: {e.Characteristic.Name}");
            System.Diagnostics.Debug.WriteLine($"Value: {e.Characteristic.Value}");
        }

        public Task<HubPropertyResponseMessage> GetPropertyUpdate(HubProperty property)
        {
            waitingForUpdate = true;

            return TaskBuilder.CreateTaskAsync<HubPropertyResponseMessage>(() =>
                {
                    var bytes = CommandCreator.CreateCommandBytes(HubProperties.Command, new byte[] { property.PropertyReference, HubProperties.PropertyOperations.RequestUpdate });
                    hubCharacteristic.WriteAsync(bytes);
                },
                (complete, reject) => (sender, args) =>
                {
                    var message = ResponseParser.ParseMessage(args.Characteristic.Value) as HubPropertyResponseMessage;
                    waitingForUpdate = false;

                    if (message == null || message.MessageType != 0x01 || message.Property != property.PropertyReference || message.Method != HubProperties.PropertyOperations.Update)
                    {
                        reject(new Exception("wrong response type"));
                        return;
                    }

                    complete(message);
                },
                hubCharacteristic);
        }

        public async Task<byte[]> GetPropertyValueAsync(HubProperty property)
        {
            var response = await GetPropertyUpdate(property).ConfigureAwait(false);

            return response == null ? new byte[0] : response.MessagePayload.ToArray();
        }

        public void Dispose()
        {
            if (hubCharacteristic != null)
            {
                hubCharacteristic.ValueUpdated -= ValueUpdated;
                hubCharacteristic = null;
            }
        }
    }

    public class HubProperty : IProperty, IPossibleOperations
    {
        public HubProperty(HubPropertyConfig config)
        {
            PropertyName = config.PropertyName;
            PropertyDescription = config.PropertyDescription;
            PropertyReference = config.PropertyReference;
            CanSet = config.CanSet;
            CanEnableUpdate = config.CanEnableUpdate;
            CanDisableUpdate = config.CanDisableUpdate;
            CanReset = config.CanReset;
            CanRequestUpdate = config.CanRequestUpdate;
            CanUpdate = config.CanUpdate;
        }

        public string PropertyName { get; }

        public string PropertyDescription { get; }

        public byte PropertyReference { get; }

        public bool CanSet { get; set; }

        public bool CanEnableUpdate { get; set; }

        public bool CanDisableUpdate { get; set; }

        public bool CanReset { get; set; }

        public bool CanRequestUpdate { get; set; }

        public bool CanUpdate { get; set; }

    }

    public static class HubPropertyExtensions
    {
        public static byte[] SetMessage(this HubProperty property, byte[] data = null)
        {
            if (property == null || !property.CanSet)
            {
                throw new NotSupportedException($"Operation 'Set' is not supported for property '{(property == null ? "unknown property" : nameof(property))}");
            }

            List<byte> byteList = new List<byte> { property.PropertyReference, 0x01 };

            if (data != null && data.Length > 0)
            {
                byteList.AddRange(data);
            }

            return byteList.ToArray();
        }
    }

    public sealed class HubPropertyConfig : IProperty, IPossibleOperations
    {
        public HubPropertyConfig(string name, string description, byte referenceByte)
        {
            PropertyName = name;
            PropertyDescription = description;
            PropertyReference = referenceByte;
        }

        public string PropertyName { get; }
        public string PropertyDescription { get; }
        public byte PropertyReference { get; }

        public bool CanSet { get; set; } = false;
        public bool CanEnableUpdate { get; set; } = false;
        public bool CanDisableUpdate { get; set; } = false;
        public bool CanReset { get; set; } = false;
        public bool CanRequestUpdate { get; set; } = true;
        public bool CanUpdate { get; set; } = true;
    }

    public interface IProperty
    {
        string PropertyName { get; }

        string PropertyDescription { get; }

        byte PropertyReference { get; }
    }

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
