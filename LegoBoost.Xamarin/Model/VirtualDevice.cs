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
    public class VirtualDevice : IVirtualDevice
    {
        private ICharacteristic hubCharacteristic;

        public Core.Model.CommunicationProtocol.Hub.AttachedIO.Type DeviceType { get; }
        
        public byte PortId { get; }
        
        public byte PortA { get; }
        
        public byte PortB { get; }

        public VirtualDevice(ICharacteristic hubCharacteristic, Core.Model.CommunicationProtocol.Hub.AttachedIO.Type type, byte portId, byte portA, byte portB)
        {
            this.hubCharacteristic = hubCharacteristic;

            DeviceType = type;
            PortId = portId;
            PortA = portA;
            PortB = portB;
        }

        public Task<HubPropertyResponseMessage> PortOutputCommandAsync(StartupAndCompletionInfo startupAndCompletionInfo, CPHub.PortOutput.SubCommands subCommand, byte[] payload)
        {
            var commandPayload = new List<byte>() { PortId, startupAndCompletionInfo.InfoByte, (byte)subCommand };
            commandPayload.AddRange(payload);

            var bytes = DataCreator.CreateCommandBytes(CPHub.PortOutput.Command, commandPayload.ToArray());

            return TaskBuilder.CreateTaskAsync<HubPropertyResponseMessage>(() =>
                {
                    hubCharacteristic.WriteAsync(bytes);
                },
                (complete, reject) => (sender, args) =>
                {
                    var response = ResponseParser.ParseMessage(args.Characteristic.Value);

                    if (response is GenericErrorResponseMessage errorResponse && errorResponse.IssuedCommand.Length > 0 && errorResponse.IssuedCommand[0] == CPHub.Property.Command)
                    {
                        reject(DataCreator.CreateExceptionFromMessage(errorResponse));
                        return;
                    }

                    if (response is HubPropertyResponseMessage message)
                    {
                        complete(message);
                    }
                },
                hubCharacteristic);
        }

    }
}