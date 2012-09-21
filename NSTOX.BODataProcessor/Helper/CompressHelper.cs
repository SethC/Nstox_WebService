using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.SharpZipLib.Zip;
using System.IO;
using ICSharpCode.SharpZipLib.Core;

namespace NSTOX.BODataProcessor.Helper
{
    public static class CompressHelper
    {
        public static List<byte[]> DeCompress(string filePath)
        {
            List<byte[]> result = new List<byte[]>();

            if (!BOFilesHelper.Exists(filePath))
                return result;

            using (var fs = BOFilesHelper.GetFileFromStorage(filePath))
            {
                using (ZipInputStream zip = new ZipInputStream(fs))
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
            }
            return result;
        }
    }
}
