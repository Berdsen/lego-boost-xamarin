using System;
using System.Threading.Tasks;
using LegoBoost.Core.Model.Constants;
using LegoBoost.Core.Model.Responses;
using LegoBoost.Core.Utilities;
using LegoBoost.Xamarin.Model.Base;
using LegoBoost.Xamarin.Utilities;
using Plugin.BLE.Abstractions.Contracts;

namespace LegoBoost.Xamarin.Model
{
    public class HubProperty : IIdentifier, IPossibleOperations
    {
        private ICharacteristic hubCharacteristic;

        public HubProperty(ICharacteristic hubCharacteristic, HubPropertyConfig config)
        {
            this.hubCharacteristic = hubCharacteristic;

            Name = config.Name;
            Description = config.Description;
            ReferenceByte = config.ReferenceByte;
            CanSet = config.CanSet;
            CanEnableUpdate = config.CanEnableUpdate;
            CanDisableUpdate = config.CanDisableUpdate;
            CanReset = config.CanReset;
            CanRequestUpdate = config.CanRequestUpdate;
            CanUpdate = config.CanUpdate;
        }

        public string Name { get; }

        public string Description { get; }

        public byte ReferenceByte { get; }

        public bool CanSet { get; set; }

        public bool CanEnableUpdate { get; set; }

        public bool CanDisableUpdate { get; set; }

        public bool CanReset { get; set; }

        public bool CanRequestUpdate { get; set; }

        public bool CanUpdate { get; set; }

        public Task<HubPropertyResponseMessage> GetPropertyUpdateAsync()
        {
            if (!CanRequestUpdate) return null;

            return TaskBuilder.CreateTaskAsync<HubPropertyResponseMessage>(() =>
                {
                    var bytes = CommandCreator.CreateCommandBytes(HubProperties.Command, new byte[] { ReferenceByte, HubProperties.PropertyOperations.RequestUpdate });
                    hubCharacteristic.WriteAsync(bytes);
                },
                (complete, reject) => (sender, args) =>
                {
                    var message = ResponseParser.ParseMessage(args.Characteristic.Value) as HubPropertyResponseMessage;

                    if (message == null || message.MessageType != 0x01 || message.Property != ReferenceByte || message.Method != HubProperties.PropertyOperations.Update)
                    {
                        reject(new Exception("wrong response type"));
                        return;
                    }

                    complete(message);
                },
                hubCharacteristic);
        }

        public async Task<byte[]> GetPropertyValueAsync()
        {
            var response = await GetPropertyUpdateAsync().ConfigureAwait(false);

            return response == null ? new byte[0] : response.MessagePayload.ToArray();
        }
        
    }
}