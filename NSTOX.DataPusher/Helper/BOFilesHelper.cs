using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Diagnostics;
using NSTOX.DataPusher.BOService;

namespace NSTOX.DataPusher.Helper
{
    public static class BOFilesHelper
    {

        private static DateTime CurrentDateTime
        {
            get
            {
                return DateTime.Now;
                //return new DateTime(2012, 04, 24);
            }
        }

        public static string ItemsFile
        {
            get
            {
                return Path.Combine(ConfigurationHelper.ItemAndDeptFilesPath, string.Format("ITEM{0}", CurrentDateTime.ToString("MMdd.yy")));
            }
        }

        public static string DepartmentsFile
        {
            get
            {
                return Path.Combine(ConfigurationHelper.ItemAndDeptFilesPath, string.Format("DEPT{0}", CurrentDateTime.ToString("MMdd.yy")));
            }
        }

        public static List<string> TransactionFiles
        {
            get
            {
                List<string> result = new List<string>();
                DirectoryInfo dirInfo = new DirectoryInfo(ConfigurationHelper.TransactionsPath);

                FileInfo[] files = dirInfo.GetFiles("*.XML");

                foreach (FileInfo file in files)
                {
                    result.Add(file.FullName);
                }

                return result;
            }
        }

        public static List<string> GetFilePathByType(BOFileType fileType, string filePath = null)
        {
            if (filePath != null)
            {
                return new List<string> { filePath };
            }

            switch (fileType)
            {
                case BOFileType.Items:
                    return new List<string>() { ItemsFile };
                case BOFileType.Departments:
                    return new List<string>() { DepartmentsFile };
                case BOFileType.Transactions:
                    return TransactionFiles;
            }

            return null;
        }

        public static byte[] Compress(List<string> files)
        {
            if (files == null || files.Count == 0)
            {
                return null;
            }

            bool atLeastOnFileExists = false;
            foreach (string file in files)
            {
                if (File.Exists(file))
                {
                    atLeastOnFileExists = true;
                    break;
                }
            }

            if (!atLeastOnFileExists)
                return null;

            int uncompressedSize = 0;
            int noOfFilesCompressed = 0;

            using (MemoryStream outputStream = new MemoryStream())
            {
                using (ZipOutputStream zip = new ZipOutputStream(outputStream))
                {
                    foreach (string file in files)
                    {
                        if (File.Exists(file))
                        {
                            noOfFilesCompressed += 1;
                            byte[] content = File.ReadAllBytes(file);

                            uncompressedSize += content.Length;

                            zip.SetLevel(ConfigurationHelper.ZipCompressionLevel);

                            ZipEntry entry = new ZipEntry(file);
                            entry.DateTime = CurrentDateTime;

                            zip.PutNextEntry(entry);

                            StreamUtils.Copy(new MemoryStream(content), zip, new byte[4096]);

                            zip.CloseEntry();
                            zip.IsStreamOwner = false;
                        }
                        else
                        {
                            Logger.LogInfo(string.Format("Coudn't find file: {0}", file), EventLogEntryType.Warning);
                        }
                    }

                    zip.Close();
                    outputStream.Position = 0;
                    return outputStream.ToArray();
                }
            }
        }

        public static List<byte[]> DeCompress(string filePath)
        {
            List<byte[]> result = new List<byte[]>();

            if (!File.Exists(filePath))
                return result;

            using (ZipInputStream zip = new ZipInputStream(new FileStream(filePath, FileMode.Open, FileAccess.Read)))
            {
                ZipEntry entry = zip.GetNextEntry();
                while (entry != null)
                {
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        StreamUtils.Copy(zip, outputStream, new byte[4096]);
                        result.Add(outputStream.ToArray());
                    }
                    entry = zip.GetNextEntry();
                }
            }
            return result;
        }
    }
}
