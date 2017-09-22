using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace StepMan
{
    public partial class Form1 : Form
    {
        private int step = 16;
        private int fullRound = 1600;

        public Form1()
        {
            InitializeComponent();

            string[] ports = SerialPort.GetPortNames();
            foreach (string p in ports)
            {
                comboBox1.Items.Add(p);
                    }

            //setSpeed("400");

        }

        private void initSerial()
        {
            serialPort1.PortName = comboBox1.Text; //"COM3";
            serialPort1.BaudRate = 9600;
            serialPort1.DtrEnable = true;
            serialPort1.Open();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setSpeed(textBox3.Text.Trim());
            reset(Convert.ToInt16(textBox1.Text), Convert.ToByte(numericUpDown13.Value), Convert.ToChar(comboBox5.Text.Substring(1, 1)) );
            start(  
                    Convert.ToByte(numericUpDown1.Value), 
                    Convert.ToByte(numericUpDown2.Value), 
                    Convert.ToByte(numericUpDown3.Value),
                    Convert.ToByte(numericUpDown12.Value),
                    Convert.ToByte(numericUpDown11.Value),
                    Convert.ToByte(numericUpDown10.Value),
                    Convert.ToByte(textBox2.Text),
                    Convert.ToByte(textBox1.Text),
                    Convert.ToChar(comboBox2.Text.Substring(1, 1)),
                    Convert.ToChar(comboBox3.Text.Substring(1, 1)),
                    Convert.ToChar(comboBox4.Text.Substring(1, 1))
                 );
            //serialPort1.Write(richTextBox1.Text);            
        }

        private void reset(int _round, byte _roundCount, char _dir)
        {
            serialPort1.Write(String.Format("{0}{1}", (_round * _roundCount), _dir));
        }

        private void start(byte first, byte second, byte thirt, byte firstRound, byte secondRound, byte thirtRound, byte _step , int _round , char firstDir, char secDir, char thDir)
        {
            int total=0;
            //1           
            serialPort1.Write(String.Format("{0}{1}", (_round * firstRound) + (first * _step), firstDir));
            //2            
            serialPort1.Write(String.Format("{0}{1}", (_round * secondRound) + (second * _step), secDir));
            //3            
            serialPort1.Write(String.Format("{0}{1}", (_round * thirtRound) + (thirt * _step), thDir));

            //open
            serialPort1.Write(String.Format("{0}{1}", (_round / 2), comboBox6.Text.Substring(1, 1)));
            
            //установка на ноль
            if (checkBox1.Checked)
            {
                total = (first * _step) - (second * _step) + (thirt * _step) - (_round / 2);
                total = (total + _round) % _round;
                if (total > 0)
                {
                    serialPort1.Write(String.Format("{0}{1}", Math.Abs(total), "R"));
                }
                else
                {
                    serialPort1.Write(String.Format("{0}{1}", Math.Abs(total), "L"));
                }
            }
        }


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (serialPort1.IsOpen) serialPort1.Close();
        }


        private void setSpeed(string speed = "300")
        {

            serialPort1.Write(String.Format("{0}S", speed));

        }

        private void button5_Click(object sender, EventArgs e)
        {
            setSpeed(textBox3.Text.Trim());
            serialPort1.Write(String.Format("{0}L", 16 * Convert.ToInt16(textBox4.Text.Trim())));
        }

        private void button6_Click(object sender, EventArgs e)
        {
            setSpeed(textBox3.Text.Trim());
            serialPort1.Write(String.Format("{0}R", 16 * Convert.ToInt16(textBox5.Text.Trim())));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            decimal first = numericUpDown1.Value;
            decimal second = numericUpDown2.Value;
            decimal thirt = numericUpDown3.Value;

            decimal firstInc = numericUpDown4.Value;
            decimal secondInc = numericUpDown5.Value;
            decimal thirtInc = numericUpDown6.Value;

            decimal firstEnd = numericUpDown7.Value;
            decimal secondEnd = numericUpDown8.Value;
            decimal thirtEnd = numericUpDown9.Value;

            setSpeed();
            while (first <= firstEnd)
            {
                second = numericUpDown2.Value;
                while (second <= secondEnd)
                {
                    thirt = numericUpDown3.Value;
                    while (thirt <= thirtEnd)
                    {
                        start (
                                Convert.ToByte(first), Convert.ToByte(second), Convert.ToByte(thirt),
                                Convert.ToByte(numericUpDown12.Value),
                                Convert.ToByte(numericUpDown11.Value),
                                Convert.ToByte(numericUpDown10.Value),
                                Convert.ToByte(textBox2.Text),
                                Convert.ToByte(textBox1.Text),
                                Convert.ToChar(comboBox2.Text.Substring(1, 1)),
                                Convert.ToChar(comboBox3.Text.Substring(1, 1)),
                                Convert.ToChar(comboBox4.Text.Substring(1, 1))
                              );
                        label12.Text = String.Format("{0} {1} {2}", first, second, thirt);
                        Application.DoEvents();
                        Thread.Sleep(1000);
                        thirt += thirtInc;
                    }
                    second += secondInc;
                }
                first += firstInc;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            initSerial();
        }
    }
}
