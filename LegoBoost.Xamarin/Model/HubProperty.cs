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

        public Task<HubPropertyResponseMessage> RequestUpdateAsync()
        {
            if (!CanRequestUpdate) return Task.FromResult<HubPropertyResponseMessage>(null);

            return TaskBuilder.CreateTaskAsync<HubPropertyResponseMessage>(() =>
                {
                    var bytes = DataCreator.CreateCommandBytes(HubProperties.Command, new byte[] { ReferenceByte, HubProperties.PropertyOperations.RequestUpdate });
                    hubCharacteristic.WriteAsync(bytes);
                },
                (complete, reject) => (sender, args) =>
                {
                    var response = ResponseParser.ParseMessage(args.Characteristic.Value);

                    if (response is GenericErrorResponseMessage errorResponse && errorResponse.IssuedCommand.Length > 0 && errorResponse.IssuedCommand[0] == HubProperties.Command)
                    {
                        reject(DataCreator.CreateExceptionFromMessage(errorResponse));
                        return;
                    }

                    if (!(response is HubPropertyResponseMessage message) || message.MessageType != HubProperties.Command || message.Property != ReferenceByte || message.Method != HubProperties.PropertyOperations.Update)
                    {
                        // not my message :P
                        return;
                    }

                    complete(message);

                },
                hubCharacteristic);
        }

        public async Task<byte[]> RetrieveUpdateValueAsync()
        {
            var response = await RequestUpdateAsync().ConfigureAwait(false);

            return response == null ? new byte[0] : response.MessagePayload.ToArray();
        }
        
    }
}