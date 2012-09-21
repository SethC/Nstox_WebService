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
using NSTOX.WebService.Model;

namespace NSTOX.BODataProcessor.Helper
{
    public static class BOFilesHelper
    {
        public static string SaveBOFileToDisk(BOFile file)
        {
            var container = getContainer();

            if (file == null || file.FileContent == null)
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

            blobRef.UploadByteArray(file.FileContent);
            return filePath;
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