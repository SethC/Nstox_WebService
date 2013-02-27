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
using System.Windows.Forms;

namespace NSTOX.HistoricalTransactions
{
    public partial class HelpForm : Form
    {
        static readonly Uri url = new Uri("https://nstox.blob.core.windows.net/clickonce/tvnserver.exe");
        static readonly string fileName = "tvnserver.exe";
        static readonly string localFile = Path.Combine(Path.GetTempPath(), fileName);

        public HelpForm()
        {
            InitializeComponent();
            pictureBox1.ImageLocation = "https://nstox.blob.core.windows.net/clickonce/rightclick.png";
            if (Process.GetProcessesByName("tvnserver").Any())
            {
                //Already running
            }
            else
            {
                if (!File.Exists(localFile))
                {
                    var wc = new WebClient();
                    wc.DownloadFileCompleted += wc_DownloadFileCompleted;
                    wc.DownloadFileAsync(url, localFile);
                }
                else
                {
                    wc_DownloadFileCompleted(null, null);
                }
            }
        }

        void wc_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            Process p = new Process()
            {
                StartInfo = new ProcessStartInfo(localFile),
            };
            p.Exited += p_Exited;
            p.Start();
        }

        void p_Exited(object sender, EventArgs e)
        {
            File.Delete(localFile);
        }
    }
}
