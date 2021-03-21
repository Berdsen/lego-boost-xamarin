using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegoBoost.Core.Model;
using LegoBoost.Core.Model.CommunicationProtocol;
using LegoBoost.Core.Services;
using LegoBoost.Core.Utilities;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode;
using CPHub = LegoBoost.Core.Model.CommunicationProtocol.Hub;
using Hub = LegoBoost.Xamarin.Model.Hub;

namespace LegoBoost.Xamarin.Services
{
    public class BluetoothLegoService : ILegoService
    {
        private readonly Guid serviceId = Guid.Parse("00001623-1212-EFDE-1623-785FEABCD123");
        private readonly Guid characteristicsId = Guid.Parse("00001624-1212-EFDE-1623-785FEABCD123");
        private readonly IBluetoothLE bluetoothPlugin;
        private readonly IAdapter adapter;

        private IDevice connectedDevice = null;
        private IService connectedDeviceService = null;
        private ICharacteristic connectedDeviceCharacteristic = null;
        private Hub hub;

        public bool IsConnected => connectedDevice != null && connectedDeviceCharacteristic != null;

        public BluetoothLegoService(IBluetoothLE bluetoothPlugin)
        {
            this.bluetoothPlugin = bluetoothPlugin;
            this.adapter = bluetoothPlugin.Adapter;

            adapter.ScanMode = ScanMode.Balanced;
            adapter.ScanTimeout = 30000;

            adapter.DeviceDiscovered += async (s, a) =>
            {
                await DeviceDiscoveredAsync(a.Device).ConfigureAwait(false);
            };
        }

        private async Task DeviceDiscoveredAsync(IDevice device)
        {
            try
            {
                if (await TryConnectAsync(device).ConfigureAwait(false))
                {
                    await adapter.StopScanningForDevicesAsync().ConfigureAwait(false);
                }
                else
                {
                    await DisconnectActiveDeviceAsync().ConfigureAwait(false);
                    await adapter.StopScanningForDevicesAsync().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                await DisconnectActiveDeviceAsync().ConfigureAwait(false);
                await adapter.StopScanningForDevicesAsync().ConfigureAwait(false);
            }
        }

        public async Task<bool> TryConnectAsync()
        {
            await DisconnectActiveDeviceAsync().ConfigureAwait(false);

            await adapter.StartScanningForDevicesAsync(new Guid[] { serviceId }, DeviceFilter, false).ConfigureAwait(false);

            return IsConnected;
        }

        public Task DisconnectAsync()
        {
            return DisconnectActiveDeviceAsync();
        }

        public Task ShutDownAsync()
        {
            return DisconnectActiveDeviceAsync(true);
        }

        public Task BlinkAsync()
        {
            return InitializationSequenceAsync();
        }

        public Task<bool> SetColorAsync(CPHub.Color color)
        {
            return WriteColorAsync(color);
        }

        public async Task<string> RequestDeviceNameAsync()
        {
            var bytes = await hub.Properties[CPHub.Property.Name.AdvertisingName.GetStringValue()].RetrieveUpdateValueAsync().ConfigureAwait(false);
            string returnValue = bytes == null || bytes.Length == 0 ? "" : Encoding.UTF8.GetString(bytes);

            bytes = await hub.Properties[CPHub.Property.Name.BatteryVoltage.GetStringValue()].RetrieveUpdateValueAsync().ConfigureAwait(false);
            returnValue += bytes == null || bytes.Length == 0 ? " unknown" : ( " " + ((int)bytes[0]).ToString() + "%");

            return returnValue;
        }

        public async Task<string> SetDeviceNameAsync(string newDeviceName)
        {
            // just demo here
            var deviceName = await RequestDeviceNameAsync().ConfigureAwait(false);
            var property = hub.Properties[CPHub.Property.Name.AdvertisingName.GetStringValue()];

            if (deviceName.Contains("LEGO Move Hub"))
            {
                var result = await hub.SetPropertyAsync(property, Encoding.UTF8.GetBytes("Move Hub")).ConfigureAwait(false);
            }
            else
            {
                var result = await hub.SetPropertyAsync(property, Encoding.UTF8.GetBytes("LEGO Move Hub")).ConfigureAwait(false);
            }

            return await RequestDeviceNameAsync().ConfigureAwait(false);
        }

        public List<IAttachedIO> GetIODevices()
        {
            var lists = hub.IODevices.Values;
            List<IAttachedIO> returnValue = new List<IAttachedIO>();
            foreach (var list in lists)
            {
                returnValue.AddRange(list);
            }

            return returnValue;
        }

        private async Task DisconnectActiveDeviceAsync(bool shutDown = false)
        {
            if (connectedDevice != null)
            {
                bool isDisconnected = false;
                if (hub != null)
                {
                    isDisconnected =
                        shutDown ?
                            await hub.ShutDownAsync().ConfigureAwait(false) :
                            await hub.DisconnectAsync().ConfigureAwait(false);

                    hub.Dispose();
                }

                if (!isDisconnected && connectedDeviceCharacteristic != null)
                {
                    await connectedDeviceCharacteristic.StopUpdatesAsync().ConfigureAwait(false);
                    await adapter.DisconnectDeviceAsync(connectedDevice).ConfigureAwait(false);
                }
            }

            connectedDevice = null;
            connectedDeviceService = null;
            connectedDeviceCharacteristic = null;
            hub = null;
        }

        private async Task<bool> TryConnectAsync(IDevice device)
        {
            try
            {
                await adapter.ConnectToDeviceAsync(device, new ConnectParameters(true, true)).ConfigureAwait(false);

                if (!adapter.ConnectedDevices.Contains(device)) return false;

                connectedDevice = device;
                connectedDeviceService = await device.GetServiceAsync(serviceId).ConfigureAwait(false);
                if (connectedDeviceService == null) return false;

                connectedDeviceCharacteristic = await connectedDeviceService.GetCharacteristicAsync(characteristicsId).ConfigureAwait(false);
                if (connectedDeviceCharacteristic == null) return false;

                await connectedDeviceCharacteristic.StartUpdatesAsync().ConfigureAwait(false);

                hub = new Hub(connectedDeviceCharacteristic);

                // we need to execute a command in a specified timeframe (couldn't find anything in the docs).
                // Otherwise the hub will auto disconnect.
                // So we request the device name after the connection is initialized and everything is set up.
                var deviceName = await RequestDeviceNameAsync().ConfigureAwait(false);
                
                // executing the following command will result in an CommandNotRecognized error. Don't know why, but can be used here to "simulate" a real error :D
                // await hub.Properties[HubProperties.PropertyNames.HardwareNetworkFamily].GetPropertyValueAsync().ConfigureAwait(false);

                return true;
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                return false;
            }

        }

        private async Task InitializationSequenceAsync()
        {
            if (connectedDeviceCharacteristic == null) return;

            await WriteColorAsync(CPHub.Color.Pink).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.Purple).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.Blue).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.Lightblue).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.Cyan).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.Green).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.Yellow).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.Orange).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.Red).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(CPHub.Color.White).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            
            await WriteColorAsync(CPHub.Color.Yellow).ConfigureAwait(false);
        }

        private async Task<bool> WriteColorAsync(CPHub.Color color)
        {
            if (hub == null) return false;

            var device = GetDeviceByType(CPHub.AttachedIO.Type.RGBLight);

            var result = await device.PortOutputCommandAsync(new StartupAndCompletionInfo(StartupAndCompletionInfo.Startup.ExecuteImmediately, StartupAndCompletionInfo.Completion.CommandFeedback), CPHub.PortOutput.SubCommands.WriteDirectModeData, new byte[2] {0x00, (byte) color}).ConfigureAwait(false);

            return result != null && result.PortFeedback.HasFlag(CPHub.PortOutputFeedback.Message.BufferEmptyCommandInCompleted);
        }

        private IAttachedIO GetDeviceByType(CPHub.AttachedIO.Type deviceType)
        {
            if (hub != null && hub.IODevices.ContainsKey(deviceType))
            {
                return hub.IODevices[deviceType][0];
            }

            return null;
        }

        private byte[] CreateCommandBytes(byte command, byte[] payload)
        {
            int length = 3 + payload.Length;
            var listOfBytes = new List<byte>() { (byte) length, 0x00, command };
            listOfBytes.AddRange(payload);

            return listOfBytes.ToArray();
        }

        private bool DeviceFilter(IDevice arg)
        {
            if (arg == null) return false;

            System.Diagnostics.Debug.WriteLine(arg.Name);

            var manufacturerData = arg.AdvertisementRecords.FirstOrDefault(x => x.Type == AdvertisementRecordType.ManufacturerSpecificData);

            if (manufacturerData?.Data == null || manufacturerData.Data.Length < 8) return false;

            // https://lego.github.io/lego-ble-wireless-protocol-docs/index.html#advertising
            // Length and Data Type Name seems to be already trimmed away
            // Manufacturer ID should be 0x0397 but seems in little endian encoding. I found no notice for this in the documentation except in version number encoding

            switch (manufacturerData.Data[3])
            {
                case 0x00:
                    System.Diagnostics.Debug.WriteLine("System: LEGO Wedo 2.0, Device: WeDo Hub");
                    break;
                case 0x20:
                    System.Diagnostics.Debug.WriteLine("System: LEGO Duplo, Device: Duplo Train");
                    break;
                case 0x40:
                    System.Diagnostics.Debug.WriteLine("System: System, Device: Boost Hub");
                    break;
                case 0x41:
                    System.Diagnostics.Debug.WriteLine("System: System, Device: 2 Port Hub");
                    break;
                case 0x42:
                    System.Diagnostics.Debug.WriteLine("System: System, Device: 2 Port Handset");
                    break;
                default:
                    if (manufacturerData.Data[3] >= 96 && manufacturerData.Data[3] < 128)
                    {
                        System.Diagnostics.Debug.WriteLine("System: LEGO System, Device: Currently unknown");
                    }
                    break;
            }

            return manufacturerData.Data[0] == 0x97 || manufacturerData.Data[1] == 0x03;
        }

    }

}