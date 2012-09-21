﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using NSTOX.WebService.Model;
using System.Web.Hosting;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using smarx.WazStorageExtensions;

namespace NSTOX.WebService.Helper
{
    public static class BOFilesHelper
    {
        public static string SaveBOFileToDisk(BOFile file)
        {
            var container = getContainer();
            
            if (file == null || file.FileContent == null)
                return string.Empty;

            string filePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, string.Format("Data\\{0}\\{1}_{2}.zip", file.RetailerId, file.FileDate.ToString("yyyyMMdd"), file.FileType));

            try
            {
                var blobRef = container.GetBlobReference(filePath);
                if (blobRef.Exists())
                {
                    string fileWOExtension = Path.GetFileNameWithoutExtension(filePath);
                    string extension = Path.GetExtension(filePath);
                    string path = Path.GetDirectoryName(filePath);
                    filePath = Path.Combine(path, fileWOExtension + DateTime.Now.ToString("HHmmss") + extension);
                    blobRef = container.GetBlobReference(filePath);
                }

                blobRef.UploadByteArray(file.FileContent);
                return filePath;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
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
    }
}