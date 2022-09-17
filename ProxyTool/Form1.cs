using System.ComponentModel;
using System.Net;
using static System.Net.WebRequestMethods;
using System.IO;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;


namespace ProxyTool
{
    public partial class Form1 : Form
    {
        string root = @"C:\ProxyTemp";
        string filepath = @"C:\ProxyTemp\proxies.txt";
        string filepath2 = @"C:\ProxyTemp\proxies2.txt";
        List<string> proxies = new List<string>();
        int a = 0;
        OpenFileDialog fo = new OpenFileDialog();
        List<string> lines = new List<string>();

        public Form1()
        {
            InitializeComponent();
            

        }

        //fetch proxy list from web
        private void button1_Click(object sender, EventArgs e)
        {
            proxies.Clear();
            lines.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();
            toolStripProgressBar1.Value = 0;
            toolStripProgressBar1.Maximum = 100;
            toolStripStatusLabel4.Text = "0";
            toolStripStatusLabel2.Text = "0";
            toolStripStatusLabel6.Text = " ";
            label4.Text = "-";
            // If directory does not exist, create it.

            if (!Directory.Exists(root))
            {
                Directory.CreateDirectory(root);
            }
            toolStripStatusLabel1.Text = "Proxylist download started";

            download1();
        }

        //Download Proxylist
        private void download1()
        {
            //download https://sunny9577.github.io/proxy-scraper/proxies.txt
            WebClient webClient = new WebClient();
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(Completed);
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(ProgressChanged);
            webClient.DownloadFileAsync(new Uri("https://sunny9577.github.io/proxy-scraper/proxies.txt"), filepath);
        }


        private void ProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }

        private void Completed(object sender, AsyncCompletedEventArgs e)
        {
            toolStripStatusLabel1.Text = "Proxylist download completed";
            label2.Text = Convert.ToString(CountLines(filepath));

            if (filepath != null)
            {
                
                using (StreamReader r = new StreamReader(filepath))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox1.Items.Add(line);

                    }
                }

                using (StreamReader sr = new StreamReader(filepath))
                {
                    while (sr.Peek() != -1)
                    {
                        proxies.Add(sr.ReadLine());
                    }
                }

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists(root))
                Directory.Delete(root, true);
        }

        private static int CountLines(string fileToCount)
        {
            int counter = 0;
            using (StreamReader countReader = new StreamReader(fileToCount))
            {
                while (countReader.ReadLine() != null)
                    counter++;
            }
            return counter;
        }


        //test
        private void button2_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = "Test Proxies";
            int b = listBox1.Items.Count;
            int c = 0;
            toolStripProgressBar1.Maximum = b;
            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel4.Text = Convert.ToString(b);
            toolStripStatusLabel2.Text = Convert.ToString(c);
            

            WebProxy myProxy = default(WebProxy);
           
            foreach (string proxy in proxies)
            {
                toolStripStatusLabel6.Text = "Please wait ...";
                if (toolStripProgressBar1.Value <= b)
                {
                    toolStripProgressBar1.Value++;
                    c++;
                    toolStripStatusLabel2.Text = Convert.ToString(c);
                }

                try
                {
                    myProxy = new WebProxy(proxy);
                    HttpWebRequest r = (HttpWebRequest)WebRequest.Create("http://www.cyrillerother.info");
                    r.UserAgent = "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/29.0.1547.2 Safari/537.36";
                    r.Timeout = 3000;
                    r.Proxy = myProxy;
                    HttpWebResponse re = (HttpWebResponse)r.GetResponse();
                    listBox2.Items.Add(proxy);
                    a++;
                    label4.Text = Convert.ToString(a);

                }
                catch (Exception)
                {

                }
            }
            toolStripStatusLabel6.Text = "Test finished";
        }

        private void button3_Click(object sender, EventArgs e)
        {

            proxies.Clear();
            lines.Clear();
            listBox1.Items.Clear();
            listBox2.Items.Clear();          
            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel4.Text = "0";
            toolStripStatusLabel2.Text = "0";
            toolStripStatusLabel6.Text = " ";
            label4.Text = "-";


            fo.RestoreDirectory = true;
            fo.Multiselect = false;
            fo.Filter = "txt files (*.txt)|*.txt";
            fo.FilterIndex = 1;
            fo.ShowDialog();
            if (fo.FileName != null)
            {
                
                using (StreamReader r = new StreamReader(fo.FileName))
                {
                    string line;
                    while ((line = r.ReadLine()) != null)
                    {
                        listBox1.Items.Add(line);

                    }
                }

                
                using (StreamReader sr = new StreamReader(fo.FileName))
                {
                    while (sr.Peek() != -1)
                    {
                        proxies.Add(sr.ReadLine());
                    }
                }
            }
            toolStripStatusLabel1.Text = "Proxylist loaded ";
            label2.Text = Convert.ToString(CountLines(fo.FileName));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (listBox2.Items.Count > 0)
            {
                SaveFileDialog fs = new SaveFileDialog();
                fs.RestoreDirectory = true;
                fs.Filter = "txt files (*.txt)|*.txt";
                fs.FilterIndex = 1;
                fs.ShowDialog();
                if (!(fs.FileName == null))
                {
                    using (StreamWriter sw = new StreamWriter(fs.FileName))
                    {
                        foreach (string line in listBox2.Items)
                        {
                            sw.WriteLine(line);
                        }
                    }

                }
            }
            toolStripStatusLabel1.Text = "---";
            toolStripProgressBar1.Value = 0;
            toolStripStatusLabel4.Text = "0";
            toolStripStatusLabel2.Text = "0";
            toolStripStatusLabel6.Text = "File saved";
        }
    }
}