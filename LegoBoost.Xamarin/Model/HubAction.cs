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
    public class HubAction : Identifier
    {
        private readonly ICharacteristic hubCharacteristic;
        private readonly byte? referenceResponseByte = null;

        public bool ExpectsResponse => referenceResponseByte.HasValue;

        public HubAction(ICharacteristic hubCharacteristic, string name, string description, byte referenceByte, byte? responseByte = null) : base(name, description, referenceByte)
        {
            this.hubCharacteristic = hubCharacteristic;
            referenceResponseByte = responseByte;
        }

        public async Task<HubActionResponseMessage> ExecuteAsync()
        {
            if (ExpectsResponse)
            {
                var result = await TaskBuilder.CreateTaskAsync<HubActionResponseMessage>(() =>
                    {
                        var bytes = CommandCreator.CreateCommandBytes(HubActions.Command, new byte[] { ReferenceByte });
                        hubCharacteristic.WriteAsync(bytes);
                    },
                    (complete, reject) => (sender, args) =>
                    {
                        var message = ResponseParser.ParseMessage(args.Characteristic.Value) as HubActionResponseMessage;

                        if (message == null || message.MessageType != HubActions.Command || message.Action != referenceResponseByte)
                        {
                            reject(new Exception("wrong response type"));
                            return;
                        }

                        complete(message);
                    },
                    hubCharacteristic).ConfigureAwait(false);

                return result;
            }
            else
            {
                var bytes = CommandCreator.CreateCommandBytes(HubActions.Command, new byte[] { ReferenceByte });
                var result = await hubCharacteristic.WriteAsync(bytes).ConfigureAwait(false);
                return new HubActionResponseMessage(bytes);

            }
        }

    }
}