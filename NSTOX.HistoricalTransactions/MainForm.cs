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

namespace NSTOX.HistoricalTransactions
{
    public partial class MainForm : Form
    {
        Thread thread = null;

        public MainForm()
        {
            InitializeComponent();
            Logger.OnLogTriggered += new OnLog(Logger_OnLogTriggered);
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
                    LoggingBox.Text += string.Format("\r\n{0}, ", msg);
                    LoggingBox.Select(LoggingBox.Text.Length, 0);
                    LoggingBox.ScrollToCaret();
                }), true);
            }
            else
            {
                LoggingBox.Text += string.Format("\r\n{0}, ", msg);
                LoggingBox.Select(LoggingBox.Text.Length, 0);
                LoggingBox.ScrollToCaret();
            }
            Application.DoEvents();
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
            if (working)
            {
                panel1.Enabled = false;
                contextMenuStrip1.Enabled = false;
                this.Cursor = Cursors.WaitCursor;
                LoggingBox.Cursor = Cursors.WaitCursor;
            }
            else
            {
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action<object>((o) =>
                    {
                        panel1.Enabled = true;
                        contextMenuStrip1.Enabled = true;
                        this.Cursor = Cursors.Arrow;
                        LoggingBox.Cursor = Cursors.Arrow;
                    }), true);
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

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoggingBox.Text = string.Empty;
        }
    }

    public class StartAndEndDate
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
