using System.Threading.Tasks;
using LegoBoost.Core.Model.Responses;
using LegoBoost.Core.Utilities;
using LegoBoost.Xamarin.Model.Base;
using LegoBoost.Xamarin.Utilities;
using Plugin.BLE.Abstractions.Contracts;

using  CPHub = LegoBoost.Core.Model.CommunicationProtocol.Hub;

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
                var commandBytes = DataCreator.CreateCommandBytes(CPHub.Action.Command, new byte[] { ReferenceByte });
                var result = await TaskBuilder.CreateTaskAsync<HubActionResponseMessage>(() =>
                    {
                        hubCharacteristic.WriteAsync(commandBytes);
                    },
                    (complete, reject) => (sender, args) =>
                    {
                        var response = ResponseParser.ParseMessage(args.Characteristic.Value);

                        if (response is GenericErrorResponseMessage errorResponse && errorResponse.IssuedCommand.Length > 0 && errorResponse.IssuedCommand[0] == CPHub.Action.Command)
                        {
                            reject(DataCreator.CreateExceptionFromMessage(errorResponse));
                            return;
                        }

                        if (!(response is HubActionResponseMessage message) || message.MessageType != CPHub.Action.Command || (byte)message.Action != referenceResponseByte)
                        {
                            // not my message :P
                            return;
                        }

                        complete(message);
                    },
                    hubCharacteristic, 5).ConfigureAwait(false);

                return result;
            }
            else
            {
                var bytes = DataCreator.CreateCommandBytes(CPHub.Action.Command, new byte[] { ReferenceByte });
                var result = await hubCharacteristic.WriteAsync(bytes).ConfigureAwait(false);
                return new HubActionResponseMessage(bytes);
            }
        }

    }
}