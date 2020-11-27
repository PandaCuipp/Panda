using Panda.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
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
            DateTime? birth = IdCardHelper.GetBirthDay(cardNo);
            double age = IdCardHelper.GetAge(cardNo);
            var sex = IdCardHelper.GetGender(cardNo);

            textBox2.Text = $"{birth:yyyy-MM-dd},{age},{sex}";

        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string cardNo = textBox1.Text.Trim();
                textBox2.Text = IdCardHelper.TransFToS(cardNo);
            }
            catch (Exception ex)
            {
                textBox2.Text = ex.Message;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBox2.Text = "" + IdCardHelper.IsIdCard(textBox1.Text.Trim());
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
                string base64 = text.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries)[0];
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

        private void button14_Click(object sender, EventArgs e)
        {
            var code = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(code))
            {
                code = "崔攀攀181613";
                textBox1.Text = "崔攀攀181613";
            }

            var len = System.Text.Encoding.Default.GetBytes(code);
            textBox2.Text = len.Length.ToString();
            var img = GetBackgroupImage(code);
            pictureBox1.Image = img;
        }
        private static Image GetBackgroupImage(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) code = "0";
            var fontSize = 16;
            var pading_left = 48;
            var pading_top = 48;

            var length = System.Text.Encoding.Default.GetByteCount(code);

            var width = length * (fontSize - 5) + pading_left * 2;
            var height = fontSize + fontSize / 2 + pading_top * 2;
            var bitMap = new Bitmap(width, height);
            using (var g1 = Graphics.FromImage(bitMap))
            {
                //填充背景图
                g1.Clear(Color.White);

                var font = new Font("微软雅黑", fontSize, FontStyle.Bold);
                var format = new StringFormat();
                format.Alignment = StringAlignment.Center;
                format.LineAlignment = StringAlignment.Center;

                var brush = new SolidBrush(Color.FromArgb(235, 235, 235));

                Matrix myMatrix = new Matrix();
                PointF pointCenter = new PointF(width / 2, height / 2);
                myMatrix.RotateAt(340, pointCenter, MatrixOrder.Prepend); //顺时针旋转330=逆时针旋转30
                g1.Transform = myMatrix;

                g1.DrawString(code, font, brush, new Rectangle(pading_left, pading_top, width - pading_left * 2, height - pading_top * 2), format);
            }

            return bitMap;
        }

        /// <summary>
        /// 旋转任一角度
        /// </summary>
        /// <param name="m_Bitmap">原图</param>
        /// <param name="angle">角色</param>
        /// <param name="isClockWise">顺时针旋转</param>
        /// <returns></returns>
        private static Bitmap Rotate(Bitmap m_Bitmap, float angle, bool isClockWise = true)
        {
            if (angle != 0)
            {
                int w = m_Bitmap.Width;
                int h = m_Bitmap.Height;

                Bitmap bmp = new Bitmap(m_Bitmap.Width, m_Bitmap.Height, PixelFormat.Format32bppArgb);//这里改为PixelFormat.Format24bppRgb也不行，而且产生了黑色背景
                Graphics g = Graphics.FromImage(bmp);
                g.Clear(Color.White);
                g.SmoothingMode = SmoothingMode.HighQuality;
                //设置高质量插值法
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
                // g.DpiX=bmp.HorizontalResolution;
                // g.DpiY=bmp.VerticalResolution;

                Matrix myMatrix = new Matrix();
                PointF pointF00 = new PointF(w / 2, h / 2);
                float dx, dy;
                if (isClockWise)
                {
                    //如果是顺时针旋转：
                    //x2 = x1 * cosa + y1 * sina
                    //y2 = y1 * cosa - x1 * sina
                    //dx=-h*(float)Math.Sin((double)f.Angle);
                    //dy=-h*(float)Math.Cos((double)f.Angle);
                    myMatrix.RotateAt(angle, pointF00);
                }
                else
                {
                    //如果是逆时针旋转：
                    //x2 = x1 * cosa - y1 * sina
                    //y2 = y1 * cosa + x1 * sina 
                    //myMatrix.Rotate((float)(-f.Angle));
                    //dx=w*(float)Math.Cos((double)f.Angle);
                    //dx=0;
                    //dy=w*(float)Math.Cos((double)f.Angle);
                    myMatrix.RotateAt((360 - angle), pointF00);
                }
                //MessageBox.Show(dx.ToString()+"\n"+dy.ToString(),"DXDY");
                //myMatrix.Translate(dx,dy);
                g.Transform = myMatrix;
                g.DrawImage(m_Bitmap, 0, 0);
                g.Dispose();

                return (Bitmap)bmp;
            }
            return m_Bitmap;
        }

        private void button15_Click(object sender, EventArgs e)
        {
            var txt = textBox1.Text.Trim();
            textBox2.Text = txt.Replace(";", "\r\n");
        }

        private void button16_Click(object sender, EventArgs e)
        {
            var txt = textBox1.Text.Trim();
            textBox2.Text = txt.Replace("\r\n", "").Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("  ", " ");
        }
    }
}
