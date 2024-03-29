﻿using log4net;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NSTOX.HistoricalTransactions
{
    public partial class MenuForm : Form
    {
        static readonly ILog log = LogManager.GetLogger(typeof(MenuForm));

        BackgroundService service = new BackgroundService();
        ClickOnceUpdater updater = new ClickOnceUpdater();
        AutoStarter starter = new AutoStarter();

        public MenuForm()
        {
            InitializeComponent();
            this.FormClosing += MainForm_FormClosing;
            service.InitializeTimer();

            log.Debug("Application Started");
        }

        void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason != CloseReason.ApplicationExitCall)
            {
                e.Cancel = true;
                ((Form)sender).Visible = false;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var mf = new MainForm())
            {
                mf.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (var sf = new SettingsForm())
            {
                sf.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var hf = new HelpForm();
            hf.Show();
        }
    }
}
