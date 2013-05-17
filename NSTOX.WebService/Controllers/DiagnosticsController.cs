using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NSTOX.WebService.Controllers
{
    public class DiagnosticsController : Controller
    {
        public ActionResult Logs(string date = null)
        {
            if (string.IsNullOrEmpty(date))
            {
                date = DateTime.Today.ToString("yyyy-MM-dd");
            }
            return View();
        }

    }
}
