using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NSTOX.DataPusher;
using NSTOX.DataPusher.Helper;
using System.Threading;
using System.IO;
using NSTOX.DataPusher.BOService;

namespace NSTOX.HistoricalTransactions
{
    public partial class MainForm : Form
    {
        Thread thread = null;

        public MainForm()
        {
            InitializeComponent();
            Logger.OnLogTriggered += new OnLog(Logger_OnLogTriggered);
            this.FormClosing += MainForm_FormClosing;
            Logger_OnLogTriggered("Service is running in the background");
            Logger_OnLogTriggered("To upload transactions from a previous day please use the controls above");
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                ((Form)sender).Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ChangeWorkingStatus(true);
            thread = new Thread(new ParameterizedThreadStart(ProcessHistoricalData));
            thread.Start(new StartAndEndDate { StartDate = StartDate.Value.Date, EndDate = EndDate.Value.Date });
        }

        void Logger_OnLogTriggered(string msg)
        {
            if (LoggingBox.InvokeRequired)
            {
                LoggingBox.Invoke(new Action<object>((o) =>
                {
                    Logger_OnLogTriggered(msg);
                }), true);
            }
            else
            {
                LoggingBox.Text += string.Format("{1}{0}, ", msg, Environment.NewLine);
                LoggingBox.Select(LoggingBox.Text.Length, 0);
                LoggingBox.ScrollToCaret();
                Application.DoEvents();
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            retailerId.Value = Convert.ToInt32(ConfigurationHelper.GetConfigValue("RetailerId"));
            retailerName.Text = ConfigurationHelper.GetConfigValue("RetailerName");
        }

        private void SaveRetailerToConfig()
        {
            ConfigurationHelper.SetConfigValue("RetailerId", retailerId.Value.ToString());
        }

        private void retailerId_Leave(object sender, EventArgs e)
        {
            SaveRetailerToConfig();
        }

        private void retailerName_Leave(object sender, EventArgs e)
        {
            ConfigurationHelper.SetConfigValue("RetailerName", retailerName.Text);
        }

        private void ChangeWorkingStatus(bool working)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<object>((o) => ChangeWorkingStatus(working)));
            }
            else
            {
                if (working)
                {
                    panel1.Enabled = false;
                    contextMenuStrip1.Enabled = false;
                    this.Cursor = Cursors.WaitCursor;
                    LoggingBox.Cursor = Cursors.WaitCursor;
                }
                else
                {
                    panel1.Enabled = true;
                    contextMenuStrip1.Enabled = true;
                    this.Cursor = Cursors.Arrow;
                    LoggingBox.Cursor = Cursors.Arrow;
                }
            }
        }

        private void ProcessHistoricalData(object data)
        {
            StartAndEndDate date = data as StartAndEndDate;
            if (date == null)
                return;
            Pusher.PushHistoricalTransactions(date.StartDate, date.EndDate);
            ChangeWorkingStatus(false);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.O))
            {
                OpenFileDialog odf = new OpenFileDialog()
                {

                };
                if (odf.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    using (var fs = File.OpenRead(odf.FileName))
                    {
                        Pusher.PushBOFileFromZip(BOFileType.Transactions, fs, StartDate.Value.Date);
                    }
                }
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoggingBox.Text = string.Empty;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }
    }

    public class StartAndEndDate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
