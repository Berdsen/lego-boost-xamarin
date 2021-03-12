using System;
using System.Threading.Tasks;
using Android.Bluetooth;
using LegoBoostDemo.Services;
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
                    await DisconnectActiveDevice().ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                await DisconnectActiveDevice().ConfigureAwait(false);
            }
        }

        public async Task<bool> TryConnectAsync()
        {
            await DisconnectActiveDevice().ConfigureAwait(false);

            await adapter.StartScanningForDevicesAsync(new Guid[] { serviceId }, DeviceFilter).ConfigureAwait(false);

            return IsConnected;
        }

        public async Task TryDisconnectAsync()
        {
            await DisconnectActiveDevice();
        }

        public Task BlinkAsync()
        {
            return InitializationSequenceAsync();
        }

        public Task SetColorAsync(BoostColors color)
        {
            return WriteColorAsync(color);
        }

        private async Task DisconnectActiveDevice()
        {
            if (connectedDevice != null)
            {
                await adapter.DisconnectDeviceAsync(connectedDevice).ConfigureAwait(false);
            }

            connectedDevice = null;
            connectedDeviceService = null;
            connectedDeviceCharacteristic = null;
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
            var bytes = new byte[] {0x08, 0x00, 0x81, 0x32, 0x11, 0x51, 0x00, (byte)color};

            return connectedDeviceCharacteristic.WriteAsync(bytes);
        }

        private bool DeviceFilter(IDevice arg)
        {
            if (arg == null) return false;

            System.Diagnostics.Debug.WriteLine(arg.Name);

            if (!(arg.Name?.ToLower() ?? "").Contains("lego")) return false;

            var bluetoothDevice = arg.NativeDevice as BluetoothDevice;
            if (bluetoothDevice?.Type == BluetoothDeviceType.Le) return true;

            return false;
        }

    }
}
