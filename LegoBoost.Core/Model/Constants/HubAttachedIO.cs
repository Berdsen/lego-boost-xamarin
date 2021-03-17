namespace LegoBoost.Core.Model.Constants
{
    public static class HubAttachedIO
    {
        public const byte Command = 0x04;

        public static class EventBytes
        {
            public const byte DetachedIO = 0x00;
            public const byte AttachedIO = 0x01;
            public const byte AttachedVirtualIO = 0x02;
        }

        public enum IOTypes : ushort
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

        public static class IOTypeIds
        {
            public const ushort Motor = 0x0001;
            public const ushort SystemTrainMotor = 0x0002;
            public const ushort Button = 0x0005;
            public const ushort LEDLight = 0x0008;
            public const ushort Voltage = 0x0014;
            public const ushort Current = 0x0015;
            public const ushort PiezoTone = 0x0016;
            public const ushort RGBLight = 0x0017;
            public const ushort ExternalTiltSensor = 0x0022;
            public const ushort MotionSensor = 0x0023;
            public const ushort VisionSensor = 0x0025;
            public const ushort ExternalMotorWithTacho = 0x0026;
            public const ushort InternalMotorWithTacho = 0x0027;
            public const ushort InternalTilt = 0x0028;

            // next one is questionable. Could not be found in official documentation
            public const ushort Speaker = 0x0042;
        }

    }
}