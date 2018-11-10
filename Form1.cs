using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TS_Downloader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        static string url;
        static int num;
        static bool Compiling = false;
        private void button1_Click(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains("0.ts"))
            {
                MessageBox.Show("Please make sure your ts file is started from 0.ts");
                return;
            }
            Directory.CreateDirectory("Download");
            url = textBox1.Text.Replace("0.ts","");
            WebClient wc = new WebClient();
            wc.DownloadFile(url + 0 + ".ts", "Download\\" + 0 + ".ts");
            Thread t = new Thread(Downloader);
            Thread t1 = new Thread(Downloader);
            Thread t2 = new Thread(Downloader);
            t2.Start();
            t.Start();
            t1.Start();
            timer1.Start();
        }

        private void Downloader()
        {
            WebClient wc = new WebClient();
            while (true)
            {
                try
                {
                    wc.DownloadFile(url + num + ".ts", "Download\\"+ num + ".ts");
                    num++;
                }
                catch (WebException wex)
                {
                    try
                    {
                        if (((HttpWebResponse)wex.Response).StatusCode == HttpStatusCode.NotFound)
                        {
                            break;
                        }
                        else
                        {
                            num++;
                        }
                    }
                    catch
                    {
                        num++;
                    }
                }
                catch
                {
                    num++;
                }
            }
            Compiling = true;
            File.WriteAllText("Download\\Run.bat", "copy /b *.ts joined_files.mp4\ndel Run.bat");
            Process proc = Process.Start("Download\\Run.bat");
            while (!proc.HasExited)
            {
                Thread.Sleep(1000);
            }
            Environment.Exit(0);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (!Compiling)
            {
                label2.Text = num + ".ts";
            }
            else
            {
                label1.Text = "Combining...";
                label2.Text = "";
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
