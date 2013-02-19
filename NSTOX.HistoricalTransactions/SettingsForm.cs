using NSTOX.DataPusher.Helper;
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
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ConfigurationHelper.SetConfigValue("RetailerId", retailerId.Value.ToString());
            ConfigurationHelper.SetConfigValue("RetailerName", retailerName.Text);
            this.Close();
        }
    }
}
