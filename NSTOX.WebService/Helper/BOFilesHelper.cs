using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using NSTOX.WebService.Model;
using System.Web.Hosting;

namespace NSTOX.WebService.Helper
{
    public static class BOFilesHelper
    {
        public static string SaveBOFileToDisk(BOFile file)
        {
            if (file == null || file.FileContent == null)
                return string.Empty;

            string filePath = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, string.Format("Data\\{0}\\{1}_{2}.zip", file.RetailerId, file.FileDate.ToString("yyyyMMdd"), file.FileType));
            string folderPath = Path.GetDirectoryName(filePath);

            try
            {
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                if (File.Exists(filePath))
                {
                    string fileWOExtension = Path.GetFileNameWithoutExtension(filePath);
                    string extension = Path.GetExtension(filePath);
                    string path = Path.GetDirectoryName(filePath);
                    filePath = Path.Combine(path, fileWOExtension + DateTime.Now.ToString("HHmmss") + extension);
                }

                File.WriteAllBytes(filePath, file.FileContent);
                return filePath;
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return string.Empty;
            }
        }
    }
}