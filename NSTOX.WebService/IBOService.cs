using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NSTOX.WebService.Model;

namespace NSTOX.WebService
{
    [ServiceContract]
    public interface IBOService
    {
        [OperationContract]
        bool PushBOFile(BOFile file);

        [OperationContract]
        bool ProcessBOFilesForRetailer(int retailerId);
    }
}
