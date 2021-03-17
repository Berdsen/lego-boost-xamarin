using LegoBoost.Core.Model.Constants;
using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LegoBoost.Core.Model.Responses;
using LegoBoost.Core.Utilities;

namespace LegoBoost.Xamarin.Model
{
    public class Hub : IDisposable
    {
        private ICharacteristic hubCharacteristic;

        public Dictionary<string, HubProperty> Properties { get; }

        public Dictionary<string, HubAction> Actions { get; }

        public Hub(ICharacteristic hubCharacteristic)
        {
            this.hubCharacteristic = hubCharacteristic;
            this.hubCharacteristic.ValueUpdated += ValueUpdated;

            Properties = new Dictionary<string, HubProperty>();
            Actions = new Dictionary<string, HubAction>();

            InitializeProperties();
            InitializeActions();
        }

        private void InitializeProperties()
        {
            Properties.Add(HubProperties.PropertyNames.AdvertisingName, new HubProperty(hubCharacteristic, new HubPropertyConfig("Adv. Name", "", HubProperties.PropertyBytes.AdvertisingName) { CanSet = true, CanEnableUpdate = true, CanDisableUpdate = true, CanReset = true }));
            Properties.Add(HubProperties.PropertyNames.Button, new HubProperty(hubCharacteristic, new HubPropertyConfig("Button", "", HubProperties.PropertyBytes.Button) { CanEnableUpdate = true, CanDisableUpdate = true }));
            Properties.Add(HubProperties.PropertyNames.FirmwareVersion, new HubProperty(hubCharacteristic, new HubPropertyConfig("FW Version", "", HubProperties.PropertyBytes.FirmwareVersion)));
            Properties.Add(HubProperties.PropertyNames.HardwareVersion, new HubProperty(hubCharacteristic, new HubPropertyConfig("HW Version", "", HubProperties.PropertyBytes.HardwareVersion)));
            Properties.Add(HubProperties.PropertyNames.Rssi, new HubProperty(hubCharacteristic, new HubPropertyConfig("RSSI", "", HubProperties.PropertyBytes.Rssi) { CanEnableUpdate = true, CanDisableUpdate = true }));
            Properties.Add(HubProperties.PropertyNames.BatteryVoltage, new HubProperty(hubCharacteristic, new HubPropertyConfig("Battery Voltage", "", HubProperties.PropertyBytes.BatteryVoltage) { CanEnableUpdate = true, CanDisableUpdate = true }));
            Properties.Add(HubProperties.PropertyNames.BatteryType, new HubProperty(hubCharacteristic, new HubPropertyConfig("Battery Type", "", HubProperties.PropertyBytes.BatteryType)));
            Properties.Add(HubProperties.PropertyNames.ManufacturerName, new HubProperty(hubCharacteristic, new HubPropertyConfig("Manufacturer Name", "", HubProperties.PropertyBytes.ManufacturerName)));
            Properties.Add(HubProperties.PropertyNames.RadioFirmwareVersion, new HubProperty(hubCharacteristic, new HubPropertyConfig("Radio Firmware Version", "", HubProperties.PropertyBytes.RadioFirmwareVersion)));
            Properties.Add(HubProperties.PropertyNames.LWPProtocolVersion, new HubProperty(hubCharacteristic, new HubPropertyConfig("LWP Protocol Version", "", HubProperties.PropertyBytes.LWPProtocolVersion)));
            Properties.Add(HubProperties.PropertyNames.SystemTypeId, new HubProperty(hubCharacteristic, new HubPropertyConfig("System Type ID", "", HubProperties.PropertyBytes.SystemTypeId)));
            Properties.Add(HubProperties.PropertyNames.HardwareNetworkId, new HubProperty(hubCharacteristic, new HubPropertyConfig("H/W NetWork ID", "", HubProperties.PropertyBytes.HardwareNetworkId) {CanSet = true, CanReset = true}));
            Properties.Add(HubProperties.PropertyNames.PrimaryMacAdress, new HubProperty(hubCharacteristic, new HubPropertyConfig("Primary MAC adress", "", HubProperties.PropertyBytes.PrimaryMacAdress)));
            Properties.Add(HubProperties.PropertyNames.SecondaryMacAdress, new HubProperty(hubCharacteristic, new HubPropertyConfig("Secondary MAC adress", "", HubProperties.PropertyBytes.SecondaryMacAdress)));
            Properties.Add(HubProperties.PropertyNames.HardwareNetworkFamily, new HubProperty(hubCharacteristic, new HubPropertyConfig("H/W Network Family", "", HubProperties.PropertyBytes.HardwareNetworkFamily) {CanSet = true}));
        }

        private void InitializeActions()
        {
            Actions.Add(HubActions.ActionNames.SwitchHubOff, new HubAction(hubCharacteristic, HubActions.ActionNames.SwitchHubOff, "", HubActions.ActionBytes.SwitchHubOff, HubActions.ActionBytes.HubWillSwitchOff));
            Actions.Add(HubActions.ActionNames.Disconnect, new HubAction(hubCharacteristic, HubActions.ActionNames.Disconnect, "", HubActions.ActionBytes.Disconnect, HubActions.ActionBytes.HubWillDisconnect));
            Actions.Add(HubActions.ActionNames.VccPortControlOn, new HubAction(hubCharacteristic, HubActions.ActionNames.VccPortControlOn, "", HubActions.ActionBytes.VccPortControlOn));
            Actions.Add(HubActions.ActionNames.VccPortControlOff, new HubAction(hubCharacteristic, HubActions.ActionNames.VccPortControlOff, "", HubActions.ActionBytes.VccPortControlOff));
            Actions.Add(HubActions.ActionNames.ActivateBusy, new HubAction(hubCharacteristic, HubActions.ActionNames.ActivateBusy, "", HubActions.ActionBytes.ActivateBusy));
            Actions.Add(HubActions.ActionNames.DeactivateBusy, new HubAction(hubCharacteristic, HubActions.ActionNames.DeactivateBusy, "", HubActions.ActionBytes.DeactivateBusy));
            Actions.Add(HubActions.ActionNames.ShutdownImmediately, new HubAction(hubCharacteristic, HubActions.ActionNames.ShutdownImmediately, "", HubActions.ActionBytes.ShutdownImmediately));
        }

        private void ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"ResponseBytes: {BitConverter.ToString(e.Characteristic.Value)}");
        }

        public async Task<bool> SetPropertyAsync(HubProperty property, byte[] data)
        {
            if (property == null || !property.CanSet) return false;

            List<byte> payLoad = new List<byte>() { property.ReferenceByte, HubProperties.PropertyOperations.Set} ;
            payLoad.AddRange(data);

            var bytes = DataCreator.CreateCommandBytes(HubProperties.Command, payLoad.ToArray());
            return await hubCharacteristic.WriteAsync(bytes).ConfigureAwait(false);
        }

        public async Task<bool> DisconnectAsync()
        {
            var result = await ExecuteActionAsync(Actions[HubActions.ActionNames.Disconnect]).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> ShutDownAsync()
        {
            var result = await ExecuteActionAsync(Actions[HubActions.ActionNames.SwitchHubOff]).ConfigureAwait(false);

            return true;
        }

        private async Task<HubActionResponseMessage> ExecuteActionAsync(HubAction action)
        {
            try
            {
                return await action.ExecuteAsync().ConfigureAwait(false);
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return null;
            }
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

    public static class HubPropertyExtensions
    {
        public static byte[] SetMessage(this HubProperty property, byte[] data = null)
        {
            if (property == null || !property.CanSet)
            {
                throw new NotSupportedException($"Operation 'Set' is not supported for property '{(property == null ? "unknown property" : nameof(property))}");
            }

            List<byte> byteList = new List<byte> { property.ReferenceByte, 0x01 };

            if (data != null && data.Length > 0)
            {
                byteList.AddRange(data);
            }

            return byteList.ToArray();
        }
    }
}
