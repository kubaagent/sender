using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;


namespace sender
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            DateTime thisDay = DateTime.Today;

            string dat;
            dat = thisDay.ToString("M");
            
            string newdata;
            StringBuilder kalbi = new StringBuilder(dat);
            kalbi.Replace(' ', '-');
            kalbi.Replace('ź', 'z');
            kalbi.Replace('ń', 'n');
            newdata = kalbi.ToString();
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8; //zmiana kodowania
            client.Encoding = UTF8Encoding.UTF8;
            string content = client.DownloadString("http://www.kalbi.pl/" + kalbi);
            string text = Regex.Match(content, @"\#8222;*(?<Title>[\s\S]*?)&#8221;\</div\>",
            RegexOptions.IgnoreCase).Groups["Title"].Value;
            
           //wysylanie maila
            try
            {
                //serwer poczty
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("poczta.o2.pl");
                //podstawowe dane
                mail.From = new MailAddress(textBox1.Text);
                mail.To.Add(textBox3.Text);
                mail.Subject = "Przysłowie na " + dat;
                mail.Body = text + "\n" + textBox4.Text;

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(textBox1.Text, textBox2.Text);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                //tekst wiadomosci
                MessageBox.Show("Wysłano przysłowie na dziś:\n" + text);
            }
            catch (Exception)
            {
                MessageBox.Show("Sprawdź poprawność adresu e-mail");
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
