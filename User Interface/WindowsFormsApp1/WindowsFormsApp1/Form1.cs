using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using ScpDriverInterface;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public struct PacketHeader
        {

            //uint8 = byte
            //uint16 = ushort
            public ushort  m_packetFormat;         // 2018
            public byte m_packetVersion;        // Version of this packet type, all start from 1
            public byte m_packetId;             // Identifier for the packet type, see below
            public ulong m_sessionUID;           // Unique identifier for the session
            public float m_sessionTime;          // Session timestamp
            public uint m_frameIdentifier;      // Identifier for the frame the data was retrieved on
           public byte m_playerCarIndex;       // Index of player's car in the array
        }; //done
   
        public struct PacketSessionData
        {
            public PacketSessionData(int a)
            {
                m_marshalZones = new MarshalZone[21];
                m_header = new PacketHeader();
                m_weather=0;                // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                               // 3 = light rain, 4 = heavy rain, 5 = storm
                 m_trackTemperature = 0;        // Track temp. in degrees celsius
                 m_airTemperature = 0;          // Air temp. in degrees celsius
                 m_totalLaps = 0;              // Total number of laps in this race
                 m_trackLength = 0;               // Track length in metres
                 m_sessionType = 0;            // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P
                                               // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ
                                               // 10 = R, 11 = R2, 12 = Time Trial
                 m_trackId = 0;                 // -1 for unknown, 0-21 for tracks, see appendix
                 m_era = 0;                    // Era, 0 = modern, 1 = classic
                 m_sessionTimeLeft = 0;       // Time left in session in seconds
                 m_sessionDuration = 0;       // Session duration in seconds
                 m_pitSpeedLimit = 0;          // Pit speed limit in kilometres per hour
                 m_gamePaused = 0;               // Whether the game is paused
                 m_isSpectating = 0;           // Whether the player is spectating
                 m_spectatorCarIndex = 0;      // Index of the car being spectated
                 m_sliProNativeSupport = 0;    // SLI Pro support, 0 = inactive, 1 = active
                 m_numMarshalZones = 0;            // Number of marshal zones to follow
                 m_safetyCarStatus = 0;          // 0 = no safety car, 1 = full safety car
                                                 // 2 = virtual safety car
                 m_networkGame = 0;              // 0 = offline, 1 = online
            }
            PacketHeader m_header;                  // Header

            byte m_weather;                // Weather - 0 = clear, 1 = light cloud, 2 = overcast
                                            // 3 = light rain, 4 = heavy rain, 5 = storm
            sbyte m_trackTemperature;        // Track temp. in degrees celsius
            sbyte m_airTemperature;          // Air temp. in degrees celsius
            byte m_totalLaps;              // Total number of laps in this race
            ushort m_trackLength;               // Track length in metres
            byte m_sessionType;            // 0 = unknown, 1 = P1, 2 = P2, 3 = P3, 4 = Short P
                                            // 5 = Q1, 6 = Q2, 7 = Q3, 8 = Short Q, 9 = OSQ
                                            // 10 = R, 11 = R2, 12 = Time Trial
            sbyte m_trackId;                 // -1 for unknown, 0-21 for tracks, see appendix
            byte m_era;                    // Era, 0 = modern, 1 = classic
            ushort m_sessionTimeLeft;       // Time left in session in seconds
            ushort m_sessionDuration;       // Session duration in seconds
            byte m_pitSpeedLimit;          // Pit speed limit in kilometres per hour
            byte m_gamePaused;               // Whether the game is paused
            byte m_isSpectating;           // Whether the player is spectating
            byte m_spectatorCarIndex;      // Index of the car being spectated
            byte m_sliProNativeSupport;    // SLI Pro support, 0 = inactive, 1 = active
            byte m_numMarshalZones;            // Number of marshal zones to follow
            MarshalZone[] m_marshalZones;      // List of marshal zones – max 21
            byte m_safetyCarStatus;          // 0 = no safety car, 1 = full safety car
                                              // 2 = virtual safety car
            byte m_networkGame;              // 0 = offline, 1 = online
        }; //done
        public struct PacketMotionData
        {
            public PacketMotionData(int a)
            {
                m_header= new PacketHeader();               // Header
                m_carMotionData = new CarMotionData[20];    // Data for all cars on track

                // Extra player car ONLY data
                m_suspensionPosition = new float[4];       // Note: All wheel arrays have the following order:
                 m_suspensionVelocity = new float[4];       // RL, RR, FL, FR
                 m_suspensionAcceleration = new float[4];    // RL, RR, FL, FR
                 m_wheelSpeed = new float[4];                // Speed of each wheel
                 m_wheelSlip = new float[4];                // Slip ratio for each wheel
                 m_localVelocityX=0;              // Velocity in local space
                 m_localVelocityY = 0;              // Velocity in local space
                 m_localVelocityZ = 0;              // Velocity in local space
                 m_angularVelocityX = 0;            // Angular velocity x-component
                 m_angularVelocityY = 0;            // Angular velocity y-component
                 m_angularVelocityZ = 0;            // Angular velocity z-component
                 m_angularAccelerationX = 0;        // Angular velocity x-component
                 m_angularAccelerationY = 0;        // Angular velocity y-component
                 m_angularAccelerationZ = 0;        // Angular velocity z-component
                 m_frontWheelsAngle = 0;            // Current front wheels angle in radians
            }
            PacketHeader m_header;               // Header

            CarMotionData[] m_carMotionData;    // Data for all cars on track

            // Extra player car ONLY data
            float[] m_suspensionPosition;       // Note: All wheel arrays have the following order:
            float[] m_suspensionVelocity;       // RL, RR, FL, FR
            float[] m_suspensionAcceleration;   // RL, RR, FL, FR
            float[] m_wheelSpeed;               // Speed of each wheel
            float[] m_wheelSlip;                // Slip ratio for each wheel
            float m_localVelocityX;              // Velocity in local space
            float m_localVelocityY;              // Velocity in local space
            float m_localVelocityZ;              // Velocity in local space
            float m_angularVelocityX;            // Angular velocity x-component
            float m_angularVelocityY;            // Angular velocity y-component
            float m_angularVelocityZ;            // Angular velocity z-component
            float m_angularAccelerationX;        // Angular velocity x-component
            float m_angularAccelerationY;        // Angular velocity y-component
            float m_angularAccelerationZ;        // Angular velocity z-component
            float m_frontWheelsAngle;            // Current front wheels angle in radians
        }; //done
        public struct PacketLapData
        {
            public PacketLapData(int a)
            {
                m_header = new PacketHeader();
                m_lapData = new LapData[20];
            }
            public PacketHeader m_header;              // Header

            public LapData[] m_lapData;         // Lap data for all cars on track
        }; //done
        public struct PacketCarTelemetryData
        {
            public PacketCarTelemetryData(int a)
            {
                m_header = new PacketHeader();
                m_carTelemetryData = new CarTelemetryData[20];
                m_buttonStatus = 0;
            }
            PacketHeader m_header;                // Header

            CarTelemetryData[] m_carTelemetryData;//20

            ulong m_buttonStatus;         // Bit flags specifying which buttons are being
                                           // pressed currently - see appendices
        };//done
        public struct MarshalZone
        {
            float m_zoneStart;   // Fraction (0..1) of way through the lap the marshal zone starts
            sbyte m_zoneFlag;    // -1 = invalid/unknown, 0 = none, 1 = green, 2 = blue, 3 = yellow, 4 = red
        };//done
        public struct CarMotionData // done
        {
            float m_worldPositionX;           // World space X position
            float m_worldPositionY;           // World space Y position
            float m_worldPositionZ;           // World space Z position
            float m_worldVelocityX;           // Velocity in world space X
            float m_worldVelocityY;           // Velocity in world space Y
            float m_worldVelocityZ;           // Velocity in world space Z
            short m_worldForwardDirX;         // World space forward X direction (normalised)
            short m_worldForwardDirY;         // World space forward Y direction (normalised)
            short m_worldForwardDirZ;         // World space forward Z direction (normalised)
            short m_worldRightDirX;           // World space right X direction (normalised)
            short m_worldRightDirY;           // World space right Y direction (normalised)
            short m_worldRightDirZ;           // World space right Z direction (normalised)
            float m_gForceLateral;            // Lateral G-Force component
            float m_gForceLongitudinal;       // Longitudinal G-Force component
            float m_gForceVertical;           // Vertical G-Force component
            float m_yaw;                      // Yaw angle in radians
            float m_pitch;                    // Pitch angle in radians
            float m_roll;                     // Roll angle in radians
        };
        public  struct LapData
        {
            public float m_lastLapTime;           // Last lap time in seconds
            public float m_currentLapTime;        // Current time around the lap in seconds
            public float m_bestLapTime;           // Best lap time of the session in seconds
            public float m_sector1Time;           // Sector 1 time in seconds
            public float m_sector2Time;           // Sector 2 time in seconds
            public float m_lapDistance;           // Distance vehicle is around current lap in metres – could
                                                  // be negative if line hasn’t been crossed yet
            public float m_totalDistance;         // Total distance travelled in session in metres – could
                                                  // be negative if line hasn’t been crossed yet
            public float m_safetyCarDelta;        // Delta in seconds for safety car
            public byte m_carPosition;           // Car race position
            public byte m_currentLapNum;         // Current lap number
            public byte m_pitStatus;             // 0 = none, 1 = pitting, 2 = in pit area
            public byte m_sector;                // 0 = sector1, 1 = sector2, 2 = sector3
            public byte m_currentLapInvalid;     // Current lap invalid - 0 = valid, 1 = invalid
            public byte m_penalties;             // Accumulated time penalties in seconds to be added
            public byte m_gridPosition;          // Grid position the vehicle started the race in
            public byte m_driverStatus;          // Status of driver - 0 = in garage, 1 = flying lap
                                                 // 2 = in lap, 3 = out lap, 4 = on track
            public byte m_resultStatus;          // Result status - 0 = invalid, 1 = inactive, 2 = active
                                           // 3 = finished, 4 = disqualified, 5 = not classified
                                           // 6 = retired
        };//done
        public struct CarTelemetryData
        {
            CarTelemetryData(int a)
            {
                m_brakesTemperature=new ushort[4];       // Brakes temperature (celsius) 4
                m_tyresSurfaceTemperature = new ushort[4]; // Tyres surface temperature (celsius)
                m_tyresInnerTemperature = new ushort[4];
                m_tyresPressure = new float[4];
                m_steer = 0;
                m_speed = 0;
                m_throttle = 0;
                m_brake = 0;
                m_clutch = 0;
                m_gear = 0;
                m_engineRPM = 0;
                m_drs = 0;
                m_revLightsPercent = 0;
                m_engineTemperature = 0;
            }
            public ushort m_speed;                      // Speed of car in kilometres per hour
            byte m_throttle;                   // Amount of throttle applied (0 to 100)
            sbyte m_steer;                      // Steering (-100 (full lock left) to 100 (full lock right))
            byte m_brake;                      // Amount of brake applied (0 to 100)
            byte m_clutch;                     // Amount of clutch applied (0 to 100)
            sbyte m_gear;                       // Gear selected (1-8, N=0, R=-1)
            ushort m_engineRPM;                  // Engine RPM
            byte m_drs;                        // 0 = off, 1 = on
            byte m_revLightsPercent;           // Rev lights indicator (percentage)
            ushort[] m_brakesTemperature;       // Brakes temperature (celsius)
            ushort[] m_tyresSurfaceTemperature; // Tyres surface temperature (celsius)
            ushort[] m_tyresInnerTemperature;   // Tyres inner temperature (celsius)
            ushort m_engineTemperature;          // Engine temperature (celsius)
            float[] m_tyresPressure;           // Tyres pressure (PSI)
        };



        public int min = 0;
        public int sec=0;
        public int min2 = 0;
        public int sec2 = 0;
        int dig1 = 0;
        int dig2 = 0;
        int dig3 = 0;
        int dig4 = 0;
        int num = 0;
       
        public string Deg = "°";
        public string perc = "%";
        byte[] fast = new byte[10];
        SerialPort _serialPort = new SerialPort("COM3",9600,Parity.None,8,StopBits.One);

        private ScpBus scpBus = new ScpBus();
        X360Controller controller = new X360Controller();
        private byte[] _outputReport = new byte[8];
        UdpClient udpClient = new UdpClient();
        public IPAddress address;
        public IPEndPoint RemoteIpEndPoint;
        
        public PacketLapData lapData;
        PacketMotionData motionData;
        PacketSessionData sessionData;
        PacketCarTelemetryData telemetryData;
        char[] name = new char[50];
        
        byte[] lapTime= new byte[2];
        byte[] speed = new byte[2];
        byte[] session = new byte[2];
        byte gear;
        byte brake;
        byte steer;
        byte throttle;




        public Form1()
        {
           
            
            InitializeComponent();
            int a = (40 + (104 * trackBar1.Value));
            string b = a.ToString()+Deg;
            textBox1.Text = b;
            int c = 10 * trackBar2.Value;
            string d = c.ToString() + perc;
            textBox2.Text = d;
            _serialPort.RtsEnable = true;
            string[] ports = SerialPort.GetPortNames();
            

            // _serialPort.PortName = ports[1];
            //_serialPort.BaudRate = Convert.ToInt32(9600 );
            // _serialPort.Parity = Parity.None;
            // _serialPort.DataBits = Convert.ToInt32( 8);
            // _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
             _serialPort.ReadTimeout = 4000;
            _serialPort.WriteTimeout =5000;
            lapData = new PacketLapData(0);
            motionData= new PacketMotionData(0);
            sessionData= new PacketSessionData(0);
            telemetryData = new PacketCarTelemetryData(0);
        }
        public void Read2()
        {

            if (udpClient.Available > 0)
            {
                Byte[] receiveUdp = udpClient.Receive(ref RemoteIpEndPoint);
                switch (receiveUdp[3])
                {
                    case 2:
                        {
                            byte id =receiveUdp[17];
                            float time=BitConverter.ToSingle(new byte[] { receiveUdp[804], receiveUdp[805], receiveUdp[806], receiveUdp[807] },0);//25,26,27,28
                            lapTime = BitConverter.GetBytes(Convert.ToUInt16(time));
                            currTime.Text = Convert.ToString(BitConverter.ToUInt16(lapTime,0));
                            //  _serialPort.Write("L" + BitConverter.ToString(lapTime, 0));
                            time = BitConverter.ToSingle(new byte[] { receiveUdp[800], receiveUdp[801], receiveUdp[802], receiveUdp[803] }, 0);//25,26,27,28
                            lapTime = BitConverter.GetBytes(Convert.ToUInt16(time));
                            lastTime.Text = Convert.ToString(BitConverter.ToUInt16(lapTime, 0));
                            time = BitConverter.ToSingle(new byte[] { receiveUdp[808], receiveUdp[809], receiveUdp[810], receiveUdp[811] }, 0);//25,26,27,28
                            lapTime = BitConverter.GetBytes(Convert.ToUInt16(time));
                            bestTime.Text = Convert.ToString(BitConverter.ToUInt16(lapTime, 0));
                            break;
                        }
                    case 6:
                        {
                             byte id = receiveUdp[17];
                            // int gear = (( id+1) * 53) - 26;
                            gear = receiveUdp[1034];
                            gearText.Text =  Convert.ToString(gear) + Environment.NewLine;
                            brake = receiveUdp[1032];
                           // gearText.Text += "Brake: " + Convert.ToString(brake) + Environment.NewLine;
                           // steer = receiveUdp[1031];
                          //  gearText.Text += "Steer: " + Convert.ToString(Convert.ToInt16((sbyte)steer)) + Environment.NewLine;
                            speed[0] = receiveUdp[1028];
                            speed[1] = receiveUdp[1029];
                            speedText.Text =  Convert.ToString(BitConverter.ToUInt16(speed,0)) + Environment.NewLine;
                            throttle= receiveUdp[1030];
                          //  gearText.Text += "Throttle: " + Convert.ToString(throttle) + Environment.NewLine;

                            switch (num) {

                                case 1:
                                    { 
                                        break;
                                    }
                                case 2:
                                    {
                                       // _serialPort.Write("B" + Convert.ToString(brake));
                                        break;
                                    }
                                case 3:
                                    {
                                       // _serialPort.Write("E" + Convert.ToString(steer));
                                        break;
                                    }
                                case 4:
                                    {
                                       // _serialPort.Write("P" + BitConverter.ToString(speed, 0, 2));
                                        break;
                                    }
                                case 5:
                                    {
                                       // _serialPort.Write("H" + Convert.ToString(throttle));
                                        break;
                                    }
                                    
                            }
                            num++;
                            if (num == 6)
                            {
                                num = 0;
                            }
                           // _serialPort.Write("R" + Convert.ToString(gear));

                            //  System.Threading.Thread.Sleep(100);



                            // System.Threading.Thread.Sleep(100);


                            //  System.Threading.Thread.Sleep(100);


                            break;
                        }

                    case 4:
                        {
                            textBox7.Text = "";
                            for (int i=0; receiveUdp[i + 1033] != 0; i++)
                            {
                                name[i] =(char) receiveUdp[i + 1034];
                                textBox7.Text += Convert.ToString(name[i]);
                            }

                            byte[] a = new byte[50];
                            for(int i = 0; i < 50; i++)
                            {
                                a[i] =(byte)name[i];
                               
                            }
                           // _serialPort.Write("N"+BitConverter.ToString(a,0));
                          //  System.Threading.Thread.Sleep(100);
                            break;
                        }
                }
              //  PacketHeader p=new PacketHeader();
               //  IntPtr pnt = Marshal.AllocHGlobal(Marshal.SizeOf(p));
               // Marshal.Copy(receiveUdp, 0, pnt,receiveUdp.Length);
               // Marshal.PtrToStructure(pnt, typeof(PacketHeader));
              //  PacketHeader anotherP;
              //  anotherP = (PacketHeader)Marshal.PtrToStructure(pnt, typeof(PacketHeader));
              //  textBox3.Text = textBox3.Text+Convert.ToString(receiveUdp.Length)+ Environment.NewLine;
                //textBox3.Text = Convert.ToString(anotherP.m_packetId);
            }
        }
    
        public void Read()
        {


            if (_serialPort.BytesToRead != 0)
            {

                for (int i = 0; i < 2; i++)
                {
                    fast[i] = (byte)_serialPort.ReadByte();
                }
                // string message = _serialPort.ReadLine();

                switch ((char)fast[0])
                {
                    case 'A':
                        {
                            if (fast[1] == 48)
                            {
                                controller.Buttons -= X360Buttons.A;
                                A.BackColor = Color.Red;
                            }
                            else
                            {
                                controller.Buttons ^= X360Buttons.A;
                                A.BackColor = Color.ForestGreen;
                            }
                            break;
                        }
                    case 'X':
                        {
                            if (fast[1] == 48)
                            {
                                controller.Buttons -= X360Buttons.X;
                                X.BackColor = Color.Red;
                            }
                            else
                            {
                                controller.Buttons ^= X360Buttons.X;
                                X.BackColor = Color.ForestGreen;
                            }
                            break;
                        }
                    case 'Y':
                        {
                            if (fast[1] == 48)
                            {
                                controller.Buttons -= X360Buttons.Y;
                                Y.BackColor = Color.Red;
                            }
                            else
                            {
                                controller.Buttons ^= X360Buttons.Y;
                                Y.BackColor = Color.ForestGreen;
                            }
                            break;
                        }
                    case 'B':
                        {
                            if (fast[1] == 48)
                            {
                                controller.Buttons -= X360Buttons.B;
                                B.BackColor = Color.Red;
                            }
                            else
                            {
                                controller.Buttons ^= X360Buttons.B;
                                B.BackColor = Color.ForestGreen;
                            }
                            break;
                        }
                    case 'S':
                        {
                            if (fast[1] == 48)
                            {

                                controller.Buttons -= X360Buttons.Start;
                                start.BackColor = Color.Red;
                            }
                            else
                            {
                                controller.Buttons ^= X360Buttons.Start;
                                start.BackColor = Color.ForestGreen;
                            }
                            break;
                        }
                    case 'R':
                        {
                            if (fast[1] == 48)
                            {
                                controller.Buttons -= X360Buttons.Back;
                                back.BackColor = Color.Red;
                            }
                            else
                            {
                                controller.Buttons ^= X360Buttons.Back;
                                back.BackColor = Color.ForestGreen;
                            }
                            break;
                        }
                    case 'U':
                        {
                            if (fast[1] == 48)
                            {
                                controller.Buttons -= X360Buttons.RightBumper;
                                RB.BackColor = Color.Red;
                            }
                            else
                            {
                                controller.Buttons ^= X360Buttons.RightBumper;
                                RB.BackColor = Color.ForestGreen;
                            }
                            break;
                        }
                    case 'D':
                        {
                            if (fast[1] == 48)
                            {
                                controller.Buttons -= X360Buttons.LeftBumper;
                                LB.BackColor = Color.Red;
                            }
                            else
                            {
                                controller.Buttons ^= X360Buttons.LeftBumper;
                                LB.BackColor = Color.ForestGreen;
                            }
                            break;
                        }

                    case 'Z':
                        {
                            controller.LeftTrigger = (byte)fast[1];

                            break;
                        }
                    case 'V':
                        {
                            controller.RightTrigger = (byte)fast[1];

                            break;
                        }
                    case 'W':
                        {
                            if (fast[1] == 48)
                            {
                                controller.LeftStickX = (sbyte)(fast[2] * 255 / 2);
                            }
                            else
                            {
                                controller.LeftStickX = (sbyte)(fast[2] * 255 / -2);
                            }


                            break;
                        }
                }
            }

           // progressBar1.Value = fast[0] *  100 / 255;
            bool result = scpBus.Report(1, controller.GetReport());

       
                
            
        }
        static float ToFloat(byte[] input)
        {
            byte[] newArray = new[] { input[2], input[3], input[0], input[1] };
            return BitConverter.ToSingle(newArray, 0);
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            int a = (40 + (104 * trackBar1.Value));
            string b = a.ToString()+Deg;
            textBox1.Text = b;
        }

        private void progressBar1_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Button a= sender as System.Windows.Forms.Button;
            if (a.BackColor == Color.Red)
            {
                _serialPort.Write(a.Name + 'y');
                a.BackColor = Color.ForestGreen;
            }
            else
            {
                _serialPort.Write(a.Name + 'n');
                a.BackColor = Color.Red;
            }
        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            int a = 10*trackBar2.Value;
            string b = a.ToString() + perc;
            textBox2.Text = b;
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }



        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string[] ArrayComPortsNames = null;
            int index = -1;
            string ComPortName = null;
            ArrayComPortsNames = SerialPort.GetPortNames();
            
            do
            {
                index += 1;
                gearText.Text += ArrayComPortsNames[index] + "\n";
            } while (!((ArrayComPortsNames[index] == ComPortName) ||
                        (index == ArrayComPortsNames.GetUpperBound(0))));


        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (!(_serialPort.IsOpen))
            {
                // _serialPort.RtsEnable=true;
                // System.IO.Ports.SerialStream..ctor(String portName, Int32 baudRate, Parity parity, Int32 dataBits, StopBits stopBits, Int32 readTimeout, Int32 writeTimeout, Handshake handshake, Boolean dtrEnable, Boolean rtsEnable, Boolean discardNull, Byte parityReplace)
                // _serialPort.BaudRate = Convert.ToInt32(14400);
                
               
                    _serialPort.Open();

                scpBus.PlugIn(1);
                timer3.Enabled = true;
                button3.Text = "Disconnect";
                button14.BackColor = Color.ForestGreen;
            }
            else
            {
                scpBus.Unplug(1);
                _serialPort.Close();
                timer3.Enabled = false;
                timer1.Enabled = false;
                timer2.Enabled = false;
                button3.Text = "Connect";
                button14.BackColor = Color.Red;
            }
            
            
            
        }

        private void button34_Click(object sender, EventArgs e)
        {

        }

        private void button33_Click(object sender, EventArgs e)
        {

        }

        private void button32_Click(object sender, EventArgs e)
        {

        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {

        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            if (button4.Text == "Start")
            {
                min = 0;
                sec = 0;
                timer1.Enabled = true;
                button4.Text = "Stop";
            }
            else
            {
                timer1.Enabled = false;
                button4.Text = "Start";
            }
            

        }

        private void tickS(object sender, EventArgs e)
        {
            if (++sec == 60)
            {
                min++;
                sec = 00;
            }
            if (sec < 10&&min<10)
            {
                textBox4.Text = "0"+Convert.ToString(min) + ":0" + Convert.ToString(sec);
            }
            else if(sec<10)
            {
                textBox4.Text = Convert.ToString(min) + ":0" + Convert.ToString(sec);
            }
            else if (min < 10)
            {
                textBox4.Text = "0" + Convert.ToString(min) + ":" + Convert.ToString(sec);
            }
            else
            {
                textBox4.Text = Convert.ToString(min) + ":" + Convert.ToString(sec);
            }

            dig1 = min / 10;
            dig2 = min % 10;
            dig3 = sec / 10;
            dig4 = sec % 10;


            _serialPort.Write('T'+Convert.ToString(dig1) + Convert.ToString(dig2) + Convert.ToString(dig3) + Convert.ToString(dig4));

        }


        private void tickT(object sender, EventArgs e)
        {
            if (++sec2 == 60)
            {
                min2++;
                sec2 = 00;
            }
            if (sec2 < 10 && min2 < 10)
            {
                textBox5.Text = "0" + Convert.ToString(min2) + ":0" + Convert.ToString(sec2);
            }
            else if (sec2 < 10)
            {
                textBox5.Text = Convert.ToString(min2) + ":0" + Convert.ToString(sec2);
            }
            else if (min2 < 10)
            {
                textBox5.Text = "0" + Convert.ToString(min2) + ":" + Convert.ToString(sec2);
            }
            else
            {
                textBox5.Text = Convert.ToString(min2) + ":" + Convert.ToString(sec2);
            }

            dig1 = min2 / 10;
            dig2 = min2 % 10;
            dig3 = sec2 / 10;
            dig4 = sec2 % 10;


            _serialPort.Write('S' + Convert.ToString(dig1) + Convert.ToString(dig2) + Convert.ToString(dig3) + Convert.ToString(dig4));

        }



        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

            if (button5.Text == "Start")
            {
                min2 = 0;
                sec2 = 0;
                timer2.Enabled = true;
                button5.Text = "Stop";
            }
            else
            {
                timer2.Enabled = false;
                button5.Text = "Start";
            }
        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            Read();
           // Read2();
        }

        private void button6_Click(object sender, EventArgs e)
        {

            if (button13.BackColor == Color.Red)
            {
                IPAddress address = IPAddress.Parse("127.0.0.1");
                EndPoint RemoteIpEndPoint = new IPEndPoint(address, 20777);
                udpClient.Client.Bind(RemoteIpEndPoint);
                button13.BackColor = Color.ForestGreen;
                timer4.Enabled = true;
            }
            else
            {
                button13.BackColor = Color.Red;
                udpClient.Client.Close();
                udpClient.Close();
                timer4.Enabled = false;
            }
            
           // textBox3.Text = "Start";


        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {

            _serialPort.Write("R" + Convert.ToString(gear));
            
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //  System.Threading.Thread.Sleep(100);
            _serialPort.Write("B" + Convert.ToString(brake));
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            _serialPort.Write("E" + Convert.ToString(steer));
           

        }

        private void button8_Click(object sender, EventArgs e)
        {

            _serialPort.Write("P" + BitConverter.ToString(speed, 0, 2));
           
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
            _serialPort.Write("H" + Convert.ToString(throttle));
        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            Read2();
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void progressBar3_Click(object sender, EventArgs e)
        {

        }
    }
}
