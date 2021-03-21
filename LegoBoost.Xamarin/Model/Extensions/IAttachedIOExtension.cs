using System.Collections.Generic;
using System.Threading.Tasks;
using LegoBoost.Core.Model;
using LegoBoost.Core.Model.CommunicationProtocol;
using LegoBoost.Core.Model.Responses;
using LegoBoost.Core.Utilities;
using LegoBoost.Xamarin.Utilities;
using Plugin.BLE.Abstractions.Contracts;

namespace LegoBoost.Xamarin.Model.Extensions
{
    public static class AttachedIOExtension
    {
        public static Task<PortOutputFeedbackResponseMessage> PortOutputCommandAsync(this IAttachedIO device, ICharacteristic hubCharacteristic, StartupAndCompletionInfo startupAndCompletionInfo, Core.Model.CommunicationProtocol.Hub.PortOutput.SubCommands subCommand, byte[] payload)
        {
            var commandPayload = new List<byte>() { device.PortId, startupAndCompletionInfo.InfoByte, (byte)subCommand };
            commandPayload.AddRange(payload);

            var bytes = DataCreator.CreateCommandBytes(Core.Model.CommunicationProtocol.Hub.MessageCommand.PortOutput, commandPayload.ToArray());

            return TaskBuilder.CreateTaskAsync<PortOutputFeedbackResponseMessage>(() =>
                {
                    hubCharacteristic.WriteAsync(bytes);
                },
                (complete, reject) => (sender, args) =>
                {
                    var response = ResponseParser.ParseMessage(args.Characteristic.Value);

                    if (response is GenericErrorResponseMessage errorResponse && errorResponse.IssuedCommand.Length > 0 && errorResponse.IssuedCommand[0] == Core.Model.CommunicationProtocol.Hub.MessageCommand.PortOutput)
                    {
                        reject(DataCreator.CreateExceptionFromMessage(errorResponse));
                        return;
                    }

                    if (response is PortOutputFeedbackResponseMessage message)
                    {
                        complete(message);
                    }
                },
                hubCharacteristic);
        }

    }
}
