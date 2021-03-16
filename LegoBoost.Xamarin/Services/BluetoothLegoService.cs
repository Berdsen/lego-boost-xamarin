using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegoBoost.Core.Model.Constants;
using LegoBoost.Core.Services;
using LegoBoost.Xamarin.Model;
using Plugin.BLE.Abstractions;
using Plugin.BLE.Abstractions.Contracts;
using ScanMode = Plugin.BLE.Abstractions.Contracts.ScanMode;

namespace LegoBoostDemo.Droid.Services
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

            adapter.ScanMode = ScanMode.LowPower;
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
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                await DisconnectActiveDeviceAsync().ConfigureAwait(false);
            }
        }

        public async Task<bool> TryConnectAsync()
        {
            await DisconnectActiveDeviceAsync().ConfigureAwait(false);

            await adapter.StartScanningForDevicesAsync(new Guid[] { serviceId }, DeviceFilter, false).ConfigureAwait(false);

            return IsConnected;
        }

        public Task TryDisconnectAsync()
        {
            return DisconnectActiveDeviceAsync();
        }

        public Task BlinkAsync()
        {
            return InitializationSequenceAsync();
        }

        public Task SetColorAsync(BoostColors color)
        {
            return WriteColorAsync(color);
        }

        public async Task<string> RequestDeviceNameAsync()
        {
            var bytes = await hub.GetPropertyValueAsync(hub.Properties[HubProperties.PropertyNames.AdvertisingName]).ConfigureAwait(false);
            string returnValue = bytes == null || bytes.Length == 0 ? "" : Encoding.UTF8.GetString(bytes);

            bytes = await hub.GetPropertyValueAsync(hub.Properties[HubProperties.PropertyNames.BatteryVoltage]).ConfigureAwait(false);
            returnValue += bytes == null || bytes.Length == 0 ? " unknown" : ( " " + ((int)bytes[0]).ToString() + "%");

            return returnValue;
        }

        private async Task DisconnectActiveDeviceAsync()
        {
            if (connectedDevice != null)
            {
                await connectedDeviceCharacteristic.StopUpdatesAsync().ConfigureAwait(false);
                await adapter.DisconnectDeviceAsync(connectedDevice).ConfigureAwait(false);
                connectedDeviceCharacteristic = null;
            }

            if (hub != null)
            {
                hub.Dispose();
                hub = null;
            }

            connectedDevice = null;
            connectedDeviceService = null;
        }

        private async Task<bool> TryConnectAsync(IDevice device)
        {
            try
            {
                connectedDevice = device;

                await adapter.ConnectToDeviceAsync(device, new ConnectParameters(true, true)).ConfigureAwait(false);
                connectedDeviceService = await device.GetServiceAsync(serviceId).ConfigureAwait(false);
                if (connectedDeviceService == null) return false;

                connectedDeviceCharacteristic = await connectedDeviceService.GetCharacteristicAsync(characteristicsId).ConfigureAwait(false);
                if (connectedDeviceCharacteristic == null) return false;

                await InitializationSequenceAsync().ConfigureAwait(false);

                await connectedDeviceCharacteristic.StartUpdatesAsync().ConfigureAwait(false);

                hub = new Hub(connectedDeviceCharacteristic);

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

            await WriteColorAsync(BoostColors.Pink).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.Purple).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.Blue).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.Lightblue).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.Cyan).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.Green).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.Yellow).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.Orange).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.Red).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);
            await WriteColorAsync(BoostColors.White).ConfigureAwait(false);
            await Task.Delay(150).ConfigureAwait(false);

            await WriteColorAsync(BoostColors.Yellow).ConfigureAwait(false);
        }

        private Task<bool> WriteColorAsync(BoostColors color)
        {
            // var bytes = new byte[] {0x08, 0x00, 0x81, 0x32, 0x11, 0x51, 0x00, (byte)color};
            var bytes = CreateCommandBytes(0x81, new byte[] {0x32, 0x11, 0x51, 0x00, (byte) color});

            return connectedDeviceCharacteristic.WriteAsync(bytes);
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