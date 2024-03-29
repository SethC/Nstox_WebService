﻿using NSTOX.DataPusher.Helper;
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
    public partial class SettingsForm : Form
    {
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void SettingsForm_Load(object sender, EventArgs e)
        {
            retailerId.Value = Convert.ToInt32(ConfigurationHelper.GetConfigValue("RetailerId"));
            retailerName.Text = ConfigurationHelper.GetConfigValue("RetailerName");
            transactionsPath.Text = ConfigurationHelper.GetConfigValue("TransactionsPath");
            txtItemsPath.Text = ConfigurationHelper.GetConfigValue("ItemAndDeptFilesPath");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigurationHelper.SetConfigValue("RetailerId", retailerId.Value.ToString());
            ConfigurationHelper.SetConfigValue("RetailerName", retailerName.Text);
            ConfigurationHelper.SetConfigValue("TransactionsPath", transactionsPath.Text);
            ConfigurationHelper.SetConfigValue("ItemAndDeptFilesPath", txtItemsPath.Text);
            this.Close();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            transactionsPath.Text = getPath(transactionsPath.Text);
        }

        private void btnItemsBrowse_Click(object sender, EventArgs e)
        {
            txtItemsPath.Text = getPath(txtItemsPath.Text);
        }

        string getPath(string startingPath)
        {
            using (FolderBrowserDialog fld = new FolderBrowserDialog())
            {
                fld.Description = "Choose the Transactions Folder";
                fld.SelectedPath = startingPath;
                fld.ShowNewFolderButton = true;
                if (fld.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    return fld.SelectedPath;
                }
            }
            return startingPath;
        }
    }
}
