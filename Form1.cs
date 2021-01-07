using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace _20201210
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        public class resp
        {
            public List<img> data { get; set; }
        }

        public class img
        {
            public string link { get; set; }
        }

        private resp GetImages(string albumHash, string clientId)
        {
            resp result = null;
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"https://api.imgur.com/3/album/{albumHash}/images");
                //Add Header 
                WebHeaderCollection webHeaderCollection = request.Headers;
                webHeaderCollection.Add("Authorization", $"Client-ID {clientId}");

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream receiveStream = response.GetResponseStream();
                StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
                string json = readStream.ReadToEnd();
                result = JsonConvert.DeserializeObject<resp>(json);
            }
            catch (Exception exp)
            {
                Console.WriteLine(exp.ToString());
                throw;
            }
            return result;
        }

        Image img1;

        private void BtnGo_Click(object sender, EventArgs e)
        {
            var m = GetImages("G1MkeJL", "b29cb06c0adff39");
            Random crandom = new Random();
            int x = crandom.Next(0, 10);

            if (m == null) 
            {
                return;
            }
            //json --> object 
            string url = m.data[x].link;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            img1 = System.Drawing.Image.FromStream(receiveStream);

            Console.WriteLine(img1.Width);
            pictureBox1.Image = img1;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        private void Btn2_Click(object sender, EventArgs e)
        {
            
            Bitmap bmp = (Bitmap)img1;
            int a = bmp.Width / 10 ;
            RectangleF rectf = new RectangleF(0, 0, bmp.Width, bmp.Height);
            Graphics g = Graphics.FromImage(bmp);
            
            g.DrawString(TB1.Text, new Font("UD Digi Kyokasho NP-B", a), Brushes.White, rectf);

            g.Flush();

            pictureBox1.Image = bmp;




        }


        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif|PnG Image|*.png|Wmf  Image|*.wmf";
            saveFileDialog1.FilterIndex = 0;
            if (pictureBox1.Image == null)
            {
                MessageBox.Show("沒有圖片啊");
            }
            else if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (pictureBox1.Image != null)
                {
                    pictureBox1.Image.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                }
            }
        }
    }
}
