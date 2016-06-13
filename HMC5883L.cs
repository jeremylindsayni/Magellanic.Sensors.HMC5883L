using Magellanic.I2C;

namespace Magellanic.Sensors.HMC5883L
{
    public class HMC5883L : AbstractI2CDevice
    {
        private const byte I2C_ADDRESS = 0x1E;

        private byte OperatingModeRegister = 0x02;

        private byte[] FirstDataRegister = new byte[] { 0x03 };

        private byte[] IdentificationRegisterA = new byte[] { 0x0A };

        private byte[] IdentificationRegisterB = new byte[] { 0x0B };

        private byte[] IdentificationRegisterC = new byte[] { 0x0C };
        
        public HMC5883L()
        {
            this.DeviceIdentifier = new byte[3] { 0x48, 0x34, 0x33 };
        }

        public override byte GetI2cAddress()
        {
            return I2C_ADDRESS;
        }

        public override byte[] GetDeviceId()
        {
            var identificationBufferA = new byte[1];
            var identificationBufferB = new byte[1];
            var identificationBufferC = new byte[1];

            this.Slave.WriteRead(IdentificationRegisterA, identificationBufferA);
            this.Slave.WriteRead(IdentificationRegisterB, identificationBufferB);
            this.Slave.WriteRead(IdentificationRegisterC, identificationBufferC);

            return new byte[3] { identificationBufferA[0], identificationBufferB[0], identificationBufferC[0] };
        }

        public void SetOperatingMode(OperatingMode operatingMode)
        {
            // convention is to specify the register first, and then the value to write to it
            var writeBuffer = new byte[2] { OperatingModeRegister, (byte)operatingMode };

            this.Slave.Write(writeBuffer);
        }

        public RawData GetRawData()
        {
            var compassData = new byte[6];

            this.Slave.WriteRead(FirstDataRegister, compassData);

            var rawDirectionData = new RawData();

            var xReading = (short)((compassData[0] << 8) | compassData[1]);
            var zReading = (short)((compassData[2] << 8) | compassData[3]);
            var yReading = (short)((compassData[4] << 8) | compassData[5]);

            rawDirectionData.X = xReading;
            rawDirectionData.Y = yReading;
            rawDirectionData.Z = zReading;

            return rawDirectionData;
        }

        public struct RawData
        {
            public int X { get; set; }
            public int Y { get; set; }
            public int Z { get; set; }
        }
    }
}
