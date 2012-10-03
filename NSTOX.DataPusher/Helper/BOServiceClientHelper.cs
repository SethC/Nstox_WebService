using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NSTOX.DataPusher.BOService;
using NSTOX.DataPusher.Model;
using Microsoft.WindowsAzure.StorageClient;
using Microsoft.WindowsAzure;

namespace NSTOX.DataPusher.Helper
{
    static class BOServiceClientHelper
    {
        public static bool PushBOFile(this BOServiceClient client, LocalFile lf)
        {
            var retVal = client.PushBOFile(lf.BOFile);

            CloudBlobClient blobStorage = new CloudBlobClient(retVal.AccountName,
                                new StorageCredentialsSharedAccessSignature(retVal.Signature));
            
            blobStorage.Timeout = TimeSpan.FromHours(1);

            var container = blobStorage.GetContainerReference(retVal.Container);
            var blob = container.GetBlobReference(retVal.Name);

            blob.UploadByteArray(lf.FileContent, defaultRequestOptions);

            return client.Uploaded(lf.BOFile.RetailerId, retVal.Name);
        }

        static BlobRequestOptions defaultRequestOptions
        {
            get
            {
                return new BlobRequestOptions()
                {
                    Timeout = TimeSpan.FromHours(1),
                };
            }
        }
    }
}
