using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using smarx.WazStorageExtensions;
using NSTOX.BODataProcessor.Model;

namespace NSTOX.BODataProcessor.Helper
{
    public static class BOFilesHelper
    {
        public static string GetFileName(BOFile file)
        {
            var container = getContainer();

            if (file == null)
                return string.Empty;

            string filePath = string.Format("Data\\{0}\\{1}_{2}.zip", file.RetailerId, file.FileDate.ToString("yyyyMMdd"), file.FileType);

            var blobRef = container.GetBlobReference(filePath);
            if (blobRef.Exists())
            {
                string path = Path.GetDirectoryName(filePath);
                string fileWOExtension = Path.GetFileNameWithoutExtension(filePath);
                string extension = Path.GetExtension(filePath);
                filePath = Path.Combine(path, fileWOExtension + DateTime.Now.ToString("HHmmss") + extension);
                blobRef = container.GetBlobReference(filePath);
            }

            return filePath;
        }

        public static AzureUploadFile GetSignature(string path)
        {
            var container = getContainer();
            var blob = container.GetBlobReference(path);

            var signature = blob.GetSharedAccessSignature(new SharedAccessPolicy()
            {
                SharedAccessStartTime = DateTime.UtcNow.AddMinutes(-1),
                SharedAccessExpiryTime = DateTime.UtcNow.AddMinutes(59),
                Permissions = SharedAccessPermissions.Write,
            });

            return new AzureUploadFile()
            {
                AccountName = container.ServiceClient.BaseUri,
                Container = container.Name,
                Name = path,
                Signature = signature
            };
        }

        private static CloudBlobContainer getContainer()
        {
            var storageAccount = CloudStorageAccount.FromConfigurationSetting("CloudConnectionString");
            var blobStorage = storageAccount.CreateCloudBlobClient();
            var container = blobStorage.GetContainerReference("files");
            return container;
        }

        public static Stream GetFileFromStorage(string filePath)
        {
            var container = getContainer();
            var blobRef = container.GetBlobReference(filePath);
            return blobRef.OpenRead();
        }

        public static bool Exists(string filePath)
        {
            var container = getContainer();
            var blobRef = container.GetBlobReference(filePath);
            return blobRef.Exists();
        }
    }
}