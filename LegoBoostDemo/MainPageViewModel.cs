using System;
using System.Threading.Tasks;
using Acr.UserDialogs;
using LegoBoostDemo.Services;
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

        public DelegateCommand ScanCommand { get; }
        public DelegateCommand DisconnectCommand { get; }
        public DelegateCommand BlinkCommand { get; }
        public DelegateCommand<object> ColorCommand { get; }

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
            DisconnectCommand = new DelegateCommand(Disconnect);
            BlinkCommand = new DelegateCommand(Blink);
            ColorCommand = new DelegateCommand<object>(SetColor);

            DisconnectCommand.ObservesCanExecute(() => IsConnected);
            BlinkCommand.ObservesCanExecute(() => IsConnected);
            ColorCommand.ObservesCanExecute(() => IsConnected);
        }

        private async void SetColor(object color)
        {
            if (color is BoostColors bc)
            {
                userDialogs.ShowLoading("Set color");
                await legoService.SetColorAsync(bc).ConfigureAwait(false);
                userDialogs.HideLoading();
            }
        }

        private async void Blink()
        {
            userDialogs.ShowLoading("Blinking");
            await legoService.BlinkAsync().ConfigureAwait(false);
            userDialogs.HideLoading();
        }

        private async void Disconnect()
        {
            userDialogs.ShowLoading("Disconnecting");
            await legoService.TryDisconnectAsync().ConfigureAwait(false);
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
