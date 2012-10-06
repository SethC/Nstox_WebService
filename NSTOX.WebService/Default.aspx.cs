using NSTOX.WebService.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NSTOX.WebService
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Error_Click(object sender, EventArgs e)
        {
            throw new ApplicationException("Test Error");
        }

        protected void Log_Click(object sender, EventArgs e)
        {
            Logger.LogInfo("Test Message");
        }

        protected void Process_Click(object sender, EventArgs e)
        {
            BOService svc = new BOService();
            svc.ProcessBOFilesForRetailer(int.Parse(txtID.Text));
        }
    }
}