using Panda.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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


        private void button11_Click(object sender, EventArgs e)
        {
            textBox2.Text = PinYinHelper.GetPYFullString(textBox1.Text).ToLower();
            
            //textBox2.Text = PinYinHelper.GetPYFirstString2(textBox1.Text);
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
            textBox2.Text = "" + IDCardHepler.IsIdCard(textBox1.Text.Trim());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string phone = textBox1.Text.Trim();
            var list = StringHelper.GetPhoneAddress(phone);
            textBox2.Text = string.Join("\r\n", list.ToArray());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            textBox1.Text = StringHelper.GetGUID().ToString();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox2.Text = DEncryptHelper.MD5(textBox1.Text);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            textBox1.Text = StringHelper.GetGUID().ToString("N");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {
                string text = textBox1.Text.Trim();
                string base64 = text.Split(new char[] {'.'}, StringSplitOptions.RemoveEmptyEntries)[0];
                byte[] arr = Convert.FromBase64String(base64);
                textBox2.Text = System.Text.Encoding.Default.GetString(arr);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
           
        }

        private void button12_Click(object sender, EventArgs e)
        {
            textBox2.Text = textBox1.Text.Length.ToString();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            //var str = "abcdefghijklmnopqrstuvwxyz1234567890";

            textBox2.Text = GetRandomString();
        }
        private string GetRandomString()
        {
            var str = "abcdefghijklmnopqrstuvwxyz1234567890";

            //获取时间戳
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1);
            //毫秒
            var timestamp = Convert.ToInt64(ts.TotalMilliseconds);     //精确到毫秒


            var list = new List<int>();
            for (var i = 6; i >= 0; i--)
            {
                var wei = timestamp / Math.Pow(36, i);
                var index = wei % 36;
                list.Add((int)index);
            }

            Random rd = new Random();
            var c7 = rd.Next(0, 36);
            list.Add((int)c7);

            var result = "";
            foreach (var index in list)
            {
                result += str[index];
            }

            return result;
        }
        //private IDictionary<string, dynamic> DecodeToken(string token)
        //{
        //    try
        //    {
        //        JWT.
        //        var dictPayload = JWT.JsonWebToken.DecodeToObject(token, CommonToken.SecretKey) as IDictionary<string, dynamic>;
        //        return dictPayload;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}
    }
}
