namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public static partial class Hub
    {
        public static class AttachedIO
        {
            public const byte Command = 0x04;

            public enum Event : byte
            {
                DetachedIO = 0x00,
                AttachedIO = 0x01,
                AttachedVirtualIO = 0x02
            }

            public enum Type : ushort
            {
                Motor = 0x0001,
                SystemTrainMotor = 0x0002,
                Button = 0x0005,
                LEDLight = 0x0008,
                Voltage = 0x0014,
                Current = 0x0015,
                PiezoTone = 0x0016,
                RGBLight = 0x0017,
                ExternalTiltSensor = 0x0022,
                MotionSensor = 0x0023,
                VisionSensor = 0x0025,
                ExternalMotorWithTacho = 0x0026,
                InternalMotorWithTacho = 0x0027,
                InternalTilt = 0x0028,

                // next one is questionable. Could not be found in official documentation
                Speaker = 0x0042
            }
        }

    }

}
