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
        public static List<byte[]> DeCompress(byte[] content)
        {
            List<byte[]> result = new List<byte[]>();

            using (ZipInputStream zip = new ZipInputStream(new MemoryStream(content)))
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
