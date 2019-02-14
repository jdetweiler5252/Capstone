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

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public int min = 0;
        public int sec=0;
        public int min2 = 0;
        public int sec2 = 0;
        int dig1 = 0;
        int dig2 = 0;
        int dig3 = 0;
        int dig4 = 0;
       
        public string Deg = "°";
        public string perc = "%";
        byte[] fast = new byte[1];
        SerialPort _serialPort = new SerialPort("COM1",9600,Parity.None,8,StopBits.One); 
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
            

            // _serialPort.PortName = ports[0];
            // _serialPort.BaudRate = Convert.ToInt32(9600 );
            // _serialPort.Parity = Parity.None;
            // _serialPort.DataBits = Convert.ToInt32( 8);
            // _serialPort.StopBits = StopBits.One;
            // _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            // _serialPort.ReadTimeout = 4000;
            //_serialPort.WriteTimeout =5000;
            //_serialPort.DtrEnable = true;
            // _serialPort.RtsEnable = true;
            //_serialPort.DiscardNull = true;



        }
         public void Read()
        {


            if(_serialPort.BytesToRead!=0)
            fast[0] = (byte)_serialPort.ReadByte();
            // string message = _serialPort.ReadLine();
            
            progressBar1.Value = fast[0] *  100 / 255;
            
            _serialPort.DiscardInBuffer();
                
            
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
                textBox3.Text += ArrayComPortsNames[index] + "\n";
            } while (!((ArrayComPortsNames[index] == ComPortName) ||
                        (index == ArrayComPortsNames.GetUpperBound(0))));


        }

        private void button3_Click(object sender, EventArgs e)
        {
            
            if (!(_serialPort.IsOpen))
            {
                _serialPort.Open();
                timer3.Enabled = true;
                button3.Text = "Disconnect";
            }
            else
            {
                _serialPort.Close();
                timer3.Enabled = false;
                timer1.Enabled = false;
                timer2.Enabled = false;
                button3.Text = "Connect";
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
        }
    }
}
