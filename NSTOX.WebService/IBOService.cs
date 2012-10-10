using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using NSTOX.BODataProcessor.Model;

namespace NSTOX.WebService
{
    [ServiceContract]
    public interface IBOService
    {
        [OperationContract]
        AzureUploadFile PushBOFile(BOFile file);

        [OperationContract]
        bool ProcessBOFilesForRetailer(int retailerId);

        [OperationContract]
        bool Uploaded(int retailerId, string filePath);

        [OperationContract]
        string CheckConnection();
    }
}
