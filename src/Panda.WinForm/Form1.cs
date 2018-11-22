﻿using Panda.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Panda.WinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = PinYinHelper.GetPYFirstString(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string cardNo = textBox1.Text.Trim();
            DateTime? birth = IDCardHepler.GetBirthDay(cardNo);
            double age = IDCardHepler.GetAge(cardNo);

            textBox2.Text = birth?.ToString("yyyy-MM-dd") + "," + age;
            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string cardNo = textBox1.Text.Trim();
                textBox2.Text = IDCardHepler.TransFToS(cardNo);
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
             textBox2.Text ="" + IDCardHepler.IsIdCard(textBox1.Text.Trim());
        }
    }
}
