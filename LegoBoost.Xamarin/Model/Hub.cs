using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LegoBoost.Core.Model;
using LegoBoost.Core.Model.Responses;
using LegoBoost.Core.Utilities;

namespace LegoBoost.Xamarin.Model
{
    public class Hub : IDisposable
    {
        private ICharacteristic hubCharacteristic;

        public Dictionary<string, HubProperty> Properties { get; }

        public Dictionary<string, HubAction> Actions { get; }

        public Dictionary<Core.Model.CommunicationProtocol.Hub.AttachedIO.Type, List<IAttachedIO>> IODevices { get; }

        public Hub(ICharacteristic hubCharacteristic)
        {
            this.hubCharacteristic = hubCharacteristic;
            this.hubCharacteristic.ValueUpdated += ValueUpdated;

            Properties = new Dictionary<string, HubProperty>();
            Actions = new Dictionary<string, HubAction>();
            IODevices = new Dictionary<Core.Model.CommunicationProtocol.Hub.AttachedIO.Type, List<IAttachedIO>>();

            InitializeProperties();
            InitializeActions();
        }

        private void InitializeProperties()
        {
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.AdvertisingName.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("Adv. Name", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.AdvertisingName) { CanSet = true, CanEnableUpdate = true, CanDisableUpdate = true, CanReset = true }));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.Button.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("Button", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.Button) { CanEnableUpdate = true, CanDisableUpdate = true }));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.FirmwareVersion.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("FW Version", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.FirmwareVersion)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.HardwareVersion.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("HW Version", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.HardwareVersion)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.Rssi.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("RSSI", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.Rssi) { CanEnableUpdate = true, CanDisableUpdate = true }));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.BatteryVoltage.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("Battery Voltage", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.BatteryVoltage) { CanEnableUpdate = true, CanDisableUpdate = true }));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.BatteryType.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("Battery Type", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.BatteryType)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.ManufacturerName.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("Manufacturer Name", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.ManufacturerName)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.RadioFirmwareVersion.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("Radio Firmware Version", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.RadioFirmwareVersion)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.LWPProtocolVersion.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("LWP Protocol Version", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.LWPProtocolVersion)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.SystemTypeId.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("System Type ID", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.SystemTypeId)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.HardwareNetworkId.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("H/W NetWork ID", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.HardwareNetworkId) {CanSet = true, CanReset = true}));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.PrimaryMacAdress.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("Primary MAC adress", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.PrimaryMacAdress)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.SecondaryMacAdress.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("Secondary MAC adress", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.SecondaryMacAdress)));
            Properties.Add(Core.Model.CommunicationProtocol.Hub.Property.Name.HardwareNetworkFamily.GetStringValue(), new HubProperty(hubCharacteristic, new HubPropertyConfig("H/W Network Family", "", (byte)Core.Model.CommunicationProtocol.Hub.Property.Name.HardwareNetworkFamily) {CanSet = true}));
        }

        private void InitializeActions()
        {
            Actions.Add(Core.Model.CommunicationProtocol.Hub.Action.Name.SwitchHubOff.GetStringValue(), new HubAction(hubCharacteristic, Core.Model.CommunicationProtocol.Hub.Action.Name.SwitchHubOff.GetStringValue(), "", (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.SwitchHubOff, (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.HubWillSwitchOff));
            Actions.Add(Core.Model.CommunicationProtocol.Hub.Action.Name.Disconnect.GetStringValue(), new HubAction(hubCharacteristic, Core.Model.CommunicationProtocol.Hub.Action.Name.Disconnect.GetStringValue(), "", (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.Disconnect, (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.HubWillDisconnect));
            Actions.Add(Core.Model.CommunicationProtocol.Hub.Action.Name.VccPortControlOn.GetStringValue(), new HubAction(hubCharacteristic, Core.Model.CommunicationProtocol.Hub.Action.Name.VccPortControlOn.GetStringValue(), "", (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.VccPortControlOn));
            Actions.Add(Core.Model.CommunicationProtocol.Hub.Action.Name.VccPortControlOff.GetStringValue(), new HubAction(hubCharacteristic, Core.Model.CommunicationProtocol.Hub.Action.Name.VccPortControlOff.GetStringValue(), "", (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.VccPortControlOff));
            Actions.Add(Core.Model.CommunicationProtocol.Hub.Action.Name.ActivateBusy.GetStringValue(), new HubAction(hubCharacteristic, Core.Model.CommunicationProtocol.Hub.Action.Name.ActivateBusy.GetStringValue(), "", (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.ActivateBusy));
            Actions.Add(Core.Model.CommunicationProtocol.Hub.Action.Name.DeactivateBusy.GetStringValue(), new HubAction(hubCharacteristic, Core.Model.CommunicationProtocol.Hub.Action.Name.DeactivateBusy.GetStringValue(), "", (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.DeactivateBusy));
            Actions.Add(Core.Model.CommunicationProtocol.Hub.Action.Name.ShutdownImmediately.GetStringValue(), new HubAction(hubCharacteristic, Core.Model.CommunicationProtocol.Hub.Action.Name.ShutdownImmediately.GetStringValue(), "", (byte)Core.Model.CommunicationProtocol.Hub.Action.Name.ShutdownImmediately));
        }

        private void ValueUpdated(object sender, CharacteristicUpdatedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"ResponseBytes: {BitConverter.ToString(e.Characteristic.Value)}");

            var responseMessage = ResponseParser.ParseMessage(e.Characteristic.Value);
            if (responseMessage is HubAttachedIOResponseMessage attachedIOMessage)
            {
                switch (attachedIOMessage.Event)
                {
                    case Core.Model.CommunicationProtocol.Hub.AttachedIO.Event.DetachedIO:
                        RemoveIODevice(attachedIOMessage);
                        break;
                    case Core.Model.CommunicationProtocol.Hub.AttachedIO.Event.AttachedIO:
                    case Core.Model.CommunicationProtocol.Hub.AttachedIO.Event.AttachedVirtualIO:
                        AddIODevice(attachedIOMessage);
                        break;
                }
            }
        }

        private void AddIODevice(HubAttachedIOResponseMessage message)
        {
            if (!IODevices.ContainsKey(message.IOTypeId))
            {
                IODevices.Add(message.IOTypeId, new List<IAttachedIO>());
            }

            if (message.Event == Core.Model.CommunicationProtocol.Hub.AttachedIO.Event.AttachedVirtualIO)
            {
                IODevices[message.IOTypeId].Add(new VirtualDevice(message.IOTypeId, message.PortId, message.PortA, message.PortB));
            }
            else
            {
                IODevices[message.IOTypeId].Add(new RealDevice(message.IOTypeId, message.PortId, message.HardwareRevision, message.SoftwareRevision));
            }
        }

        private void RemoveIODevice(HubAttachedIOResponseMessage attachedIOMessage)
        {
            
        }

        public async Task<bool> SetPropertyAsync(HubProperty property, byte[] data)
        {
            if (property == null || !property.CanSet) return false;

            List<byte> payLoad = new List<byte>() { property.ReferenceByte, (byte)Core.Model.CommunicationProtocol.Hub.Property.Operation.Set} ;
            payLoad.AddRange(data);

            var bytes = DataCreator.CreateCommandBytes(Core.Model.CommunicationProtocol.Hub.MessageCommand.Property, payLoad.ToArray());
            return await hubCharacteristic.WriteAsync(bytes).ConfigureAwait(false);
        }

        public async Task<bool> DisconnectAsync()
        {
            var result = await ExecuteActionAsync(Actions[Core.Model.CommunicationProtocol.Hub.Action.Name.Disconnect.GetStringValue()]).ConfigureAwait(false);

            return true;
        }

        public async Task<bool> ShutDownAsync()
        {
            var result = await ExecuteActionAsync(Actions[Core.Model.CommunicationProtocol.Hub.Action.Name.SwitchHubOff.GetStringValue()]).ConfigureAwait(false);

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

            Properties?.Clear();
            Actions?.Clear();
            IODevices?.Clear();
        }
    }

}
