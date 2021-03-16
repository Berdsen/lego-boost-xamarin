using System.Diagnostics;
using Acr.UserDialogs;
using LegoBoost.Core.Services;
using LegoBoostDemo.Droid.Services;
using Plugin.BLE;
using Plugin.BLE.Abstractions.Contracts;
using Prism;
using Prism.Ioc;
using Prism.Unity;

namespace LegoBoostDemo
{
    public partial class App : PrismApplication
    {
        public App(IPlatformInitializer platformInitializer) : base(platformInitializer)
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IBluetoothLE>(CrossBluetoothLE.Current);
            containerRegistry.RegisterInstance<IUserDialogs>(UserDialogs.Instance);

            containerRegistry.RegisterSingleton<ILegoService, BluetoothLegoService>();

            // base navigation
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
        }

        protected override void OnInitialized()
        {

        }

        protected async override void OnStart()
        {
            var result = await NavigationService.NavigateAsync(nameof(MainPage));
#if DEBUG
            if (result.Exception != null)
            {
                Debugger.Break();
            }
#endif
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }


    }
}
