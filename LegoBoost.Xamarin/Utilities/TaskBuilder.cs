using Plugin.BLE.Abstractions.Contracts;
using Plugin.BLE.Abstractions.EventArgs;
using System;
using System.Threading.Tasks;

namespace LegoBoost.Xamarin.Utilities
{
    public class TaskBuilder
    {
        #region Task creation and event handling

        public static Task<T> CreateTaskAsync<T>(Action executeAction, Func<Action<T>, Action<Exception>, EventHandler<CharacteristicUpdatedEventArgs>> getCompleteHandler, ICharacteristic characteristicToUse)
        {
            return Plugin.BLE.Abstractions.Utils.TaskBuilder.FromEvent(
                executeAction,
                getCompleteHandler,
                (handler) => SubscribeReceiver(handler, characteristicToUse),
                (handler) => UnsubscribeReceiver(handler, characteristicToUse));
        }

        private static void SubscribeReceiver(EventHandler<CharacteristicUpdatedEventArgs> handler, ICharacteristic characteristic)
        {
            characteristic.ValueUpdated += handler;
        }

        private static void UnsubscribeReceiver(EventHandler<CharacteristicUpdatedEventArgs> handler, ICharacteristic characteristic)
        {
            characteristic.ValueUpdated -= handler;
        }

        #endregion
    }
}
