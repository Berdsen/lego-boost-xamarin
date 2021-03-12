using LegoBoostDemo.Droid.Services;
using LegoBoostDemo.Services;
using Plugin.Permissions;
using Plugin.Permissions.Abstractions;
using Prism;
using Prism.Ioc;

namespace LegoBoostDemo.Droid
{
    public class AndroidInitializer : IPlatformInitializer
    {
        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterInstance<IPermissions>(CrossPermissions.Current);
            containerRegistry.RegisterSingleton<ILegoService, BluetoothLegoService>();
        }
    }
}