﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NSTOX.WebService
{
    public partial class Debug : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Error_Click(object sender, EventArgs e)
        {
            throw new ApplicationException("Test Error");
        }

        protected void Process_Click(object sender, EventArgs e)
        {
            BOService svc = new BOService();
            svc.ProcessBOFilesForRetailer(int.Parse(txtID.Text));
        }
    }
}