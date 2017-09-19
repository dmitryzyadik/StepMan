using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            serialPort1.PortName = "COM3";
            serialPort1.BaudRate = 9600;
            serialPort1.DtrEnable = true;
            serialPort1.Open();
            //setSpeed("400");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            setSpeed(textBox3.Text.Trim());
            reset();
            start( numericUpDown1.Value, numericUpDown2.Value, numericUpDown3.Value);

            //serialPort1.Write(richTextBox1.Text);            
        }

        private void reset()
        {
            serialPort1.Write(String.Format("{0}{1}", (fullRound * 4) , "L"));
        }

        private void start(decimal first , decimal second , decimal thirt)
        {       
            //1           
            serialPort1.Write(String.Format("{0}{1}", (fullRound*4)+(first*step), "L"));
            //2            
            serialPort1.Write(String.Format("{0}{1}", (fullRound*3) + (second*step), "R"));
            //3            
            serialPort1.Write(String.Format("{0}{1}", (fullRound*2) + (thirt*step), "L"));

            //open
            serialPort1.Write(String.Format("{0}{1}", (fullRound / 2) , "R"));
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
            serialPort1.Write(String.Format("{0}L", 16* Convert.ToInt16(textBox4.Text.Trim())));
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
                        start(first, second,thirt);
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
    }
}
