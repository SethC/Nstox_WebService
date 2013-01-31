using Microsoft.Win32;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSTOX.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            DeployClickOnce();
        }

        static string dir = "..\\..\\..\\NSTOX.HistoricalTransactions\\bin\\Debug\\app.publish";

        private static void DeployClickOnce()
        {
            if (Directory.Exists(dir))
            {
                DirectoryInfo di = new DirectoryInfo(dir);
                dir = di.FullName;

                CloudBlobClient cbc = new CloudBlobClient("http://nstox.blob.core.windows.net",
                    new StorageCredentialsAccountAndKey("nstox", "agyTF2fhNFI/511qsodN1dqJ+LEUiA4FreiPtUWLjD0uf6yoBJnTHOYW73g6fT7TirXdVVbVPA0q0mYmh0+N7w=="));

                var clickonce = cbc.GetContainerReference("clickonce");

                foreach (var file in Directory.GetFiles(dir, "*", SearchOption.AllDirectories).Reverse())
                {
                    //var fi = new FileInfo(file);
                    Console.WriteLine("Uploading " + file);
                    var blobName = file.Replace(dir + "\\", "nstoxapp\\").Replace("\\", "/");
                    var blobRef = clickonce.GetBlobReference(blobName);
                    blobRef.UploadFile(file);
                    blobRef.Properties.ContentType = GetContentType(file);
                    blobRef.SetProperties();
                    File.Delete(file);
                }
            }
        }

        private static string GetContentType(string file)
        {
            string contentType = "application/octet-stream";
            string fileExt = System.IO.Path.GetExtension(file).ToLowerInvariant();
            RegistryKey fileExtKey = Registry.ClassesRoot.OpenSubKey(fileExt);
            if (fileExtKey != null && fileExtKey.GetValue("Content Type") != null)
            {
                contentType = fileExtKey.GetValue("Content Type").ToString();
            }
            else
            {
                switch (fileExt)
                {
                    case ".manifest":
                        contentType = "application/x-ms-manifest";
                        break;
                    default:
                        break;
                }
            }
            return contentType;
        }
    }
}
