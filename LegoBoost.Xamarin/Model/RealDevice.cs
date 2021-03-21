using System.Collections.Generic;
using System.Threading.Tasks;
using LegoBoost.Core.Model;
using LegoBoost.Core.Model.CommunicationProtocol;
using LegoBoost.Core.Model.Responses;
using LegoBoost.Core.Utilities;
using LegoBoost.Xamarin.Utilities;
using Plugin.BLE.Abstractions.Contracts;
using CPHub = LegoBoost.Core.Model.CommunicationProtocol.Hub;

namespace LegoBoost.Xamarin.Model
{
    public class RealDevice : IRealDevice
    {
        private ICharacteristic hubCharacteristic;

        public Core.Model.CommunicationProtocol.Hub.AttachedIO.Type DeviceType { get; }

        public byte PortId { get; }

        public Version HardwareVersion { get; }

        public Version SoftwareVersion { get; }

        public RealDevice(ICharacteristic hubCharacteristic, CPHub.AttachedIO.Type type, byte portId, Version hwVersion, Version swVersion)
        {
            this.hubCharacteristic = hubCharacteristic;

            DeviceType = type;
            PortId = portId;
            HardwareVersion = hwVersion;
            SoftwareVersion = swVersion;
        }

        public Task<PortOutputFeedbackResponseMessage> PortOutputCommandAsync(StartupAndCompletionInfo startupAndCompletionInfo, CPHub.PortOutput.SubCommands subCommand, byte[] payload)
        {
            var commandPayload = new List<byte>() { PortId, startupAndCompletionInfo.InfoByte, (byte)subCommand };
            commandPayload.AddRange(payload);

            var bytes = DataCreator.CreateCommandBytes(CPHub.MessageCommand.PortOutput, commandPayload.ToArray());

            return TaskBuilder.CreateTaskAsync<PortOutputFeedbackResponseMessage>(() =>
                {
                    hubCharacteristic.WriteAsync(bytes);
                },
                (complete, reject) => (sender, args) =>
                {
                    var response = ResponseParser.ParseMessage(args.Characteristic.Value);

                    if (response is GenericErrorResponseMessage errorResponse && errorResponse.IssuedCommand.Length > 0 && errorResponse.IssuedCommand[0] == CPHub.MessageCommand.PortOutput)
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