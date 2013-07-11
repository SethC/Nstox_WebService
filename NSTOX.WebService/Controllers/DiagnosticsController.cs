using CloudTraceListener;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

            var csa = CloudStorageAccount.FromConfigurationSetting("CloudConnectionString");
            var ctc = new CloudTableClient(csa.TableEndpoint, csa.Credentials);
            var ctx = ctc.GetDataServiceContext();
            var logs = ctx.CreateQuery<TraceRecord>("logs")
                .Where(a => a.PartitionKey == date)
                .ToArray()
                .OrderByDescending(a => a.Timestamp);
            return View(logs);
        }

        [HttpPost]
        public ActionResult LogMsg(string date, string level, string message, string exception)
        {
            string msg = string.Join(" ", message, exception);
            Trace.WriteLine("CLIENT " + level + ": " + msg);
            return new EmptyResult();
        }
    }
}
