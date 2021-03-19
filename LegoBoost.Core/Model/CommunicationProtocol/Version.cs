namespace LegoBoost.Core.Model.CommunicationProtocol
{
    public class Version
    {
        public int Major { get; } 
        
        public int Minor { get; } 
        
        public int Bugfix { get; }

        public int Build { get; }

        public Version(byte byte0, byte byte1, byte byte2, byte byte3)
        {
            // in the docs it's clarified, that only numeric values are allowed e.g Major 0 - 9, etc.
            // but I'll ignore the checks here because they should be irrelevant :)
            Major = byte3 >> 4;
            Minor = byte3 & 0x0F;
            Bugfix = byte2;
            Build = (byte1 << 8) | byte0;
        }

        public override string ToString()
        {
            return $"{Major.ToString("X")}.{Minor.ToString("X")}.{Bugfix.ToString("X2")}.{Build.ToString("X4")}";
        }
    }
}
