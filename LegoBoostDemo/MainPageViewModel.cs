using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using LegoBoost.Core.Model.CommunicationProtocol;
using LegoBoost.Core.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism.Commands;
using Prism.Mvvm;
using Xamarin.Forms;
using PermissionStatus = Plugin.Permissions.Abstractions.PermissionStatus;

namespace LegoBoostDemo
{
    public class MainPageViewModel : BindableBase
    {
        private ILegoService legoService;
        private IPermissions permissions;
        private IUserDialogs userDialogs;
        private bool isConnected;

        public DelegateCommand<object> DisconnectCommand { get; }
        public DelegateCommand<object> ColorCommand { get; }
        public DelegateCommand ScanCommand { get; }
        public DelegateCommand BlinkCommand { get; }
        public DelegateCommand RequestDeviceNameCommand { get; }
        public DelegateCommand SwitchDeviceNameCommand { get; }

        public bool IsConnected
        {
            get => isConnected;
            set
            {
                isConnected = value;
                RaisePropertyChanged(nameof(IsConnected));
            }
        }

        public MainPageViewModel(ILegoService legoService, IPermissions permissions, IUserDialogs userDialogs)
        {
            this.legoService = legoService;
            this.permissions = permissions;
            this.userDialogs = userDialogs;

            ScanCommand = new DelegateCommand(Scan);
            DisconnectCommand = new DelegateCommand<object>(Disconnect);
            ColorCommand = new DelegateCommand<object>(SetColor);
            BlinkCommand = new DelegateCommand(Blink);
            RequestDeviceNameCommand = new DelegateCommand(RequestDeviceName);
            SwitchDeviceNameCommand = new DelegateCommand(SwitchDeviceName);

            DisconnectCommand.ObservesCanExecute(() => IsConnected);
            ColorCommand.ObservesCanExecute(() => IsConnected);
            BlinkCommand.ObservesCanExecute(() => IsConnected);
            RequestDeviceNameCommand.ObservesCanExecute(() => IsConnected);
            SwitchDeviceNameCommand.ObservesCanExecute(() => IsConnected);
        }

        private async void RequestDeviceName()
        {
            userDialogs.ShowLoading("Requesting device name");
            var deviceName= await legoService.RequestDeviceNameAsync().ConfigureAwait(false);
            userDialogs.Toast(deviceName, TimeSpan.FromSeconds(3));
            userDialogs.HideLoading();
        }

        private async void SwitchDeviceName()
        {
            userDialogs.ShowLoading("Switching device name");
            var deviceName = await legoService.SetDeviceNameAsync("").ConfigureAwait(false);
            userDialogs.Toast(deviceName, TimeSpan.FromSeconds(3));
            userDialogs.HideLoading();
        }

        private async void SetColor(object color)
        {
            userDialogs.ShowLoading("Set color");
            await legoService.SetColorAsync((Hub.Color)color).ConfigureAwait(false);
            userDialogs.HideLoading();
        }

        private async void Blink()
        {
            userDialogs.ShowLoading("Blinking");
            await legoService.BlinkAsync().ConfigureAwait(false);
            userDialogs.HideLoading();
        }

        private async void Disconnect(object shutdown)
        {
            userDialogs.ShowLoading("Disconnecting");
            if (shutdown is bool b && b)
                await legoService.ShutDownAsync().ConfigureAwait(false);
            else
                await legoService.DisconnectAsync().ConfigureAwait(false);

            userDialogs.HideLoading();
            IsConnected = false;

            userDialogs.Toast("Disconnected", TimeSpan.FromSeconds(5));
        }

        public async void Scan()
        {
            if (!await  HasLocationPermissionAsync().ConfigureAwait(false)) return;

            userDialogs.ShowLoading("Trying to connect");
            IsConnected = await legoService.TryConnectAsync().ConfigureAwait(false);
            userDialogs.HideLoading();

            userDialogs.Toast(IsConnected ? "Successfully connected" : "Connection failed", TimeSpan.FromSeconds(5));
        }

        private async Task<bool> HasLocationPermissionAsync()
        {
            if (Xamarin.Forms.Device.RuntimePlatform == Device.Android)
            {
                var status = await permissions.CheckPermissionStatusAsync<LocationPermission>().ConfigureAwait(false);
                if (status != PermissionStatus.Granted)
                {
                    bool shouldShow = await permissions.ShouldShowRequestPermissionRationaleAsync(Permission.Location).ConfigureAwait(false);
                    if (!shouldShow)
                    {
                        permissions.OpenAppSettings();
                        return false;
                    }

                    var permissionResult = await permissions.RequestPermissionAsync<LocationPermission>().ConfigureAwait(false);

                    if (permissionResult != PermissionStatus.Granted)
                    {
                        permissions.OpenAppSettings();
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
