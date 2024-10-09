using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;

namespace adoptiumjdk_sklauncher_installer.utils
{
    internal class Downloader
    {
        public async Task Download(DownloadParams downloadParams)
        {
            try {
                Console.WriteLine($"Downloading {downloadParams.debug_downloadTitle}.");
                await DownloadFileAsync(downloadParams.downloadUrl, downloadParams.downloadFileName);
                Console.WriteLine("Download complete.");

                Console.WriteLine($"Installing {downloadParams.debug_downloadTitle}");
                ExtractZipFile(downloadParams.downloadFileName, downloadParams.extractPath);
                Console.WriteLine($"Done.");

                // Optional: Delete the downloaded file
                File.Delete(downloadParams.downloadFileName);
                Console.WriteLine($"{downloadParams.debug_downloadTitle} Clean-up completed.");
            }
            catch {
                
            }
        }

        // Method to download the file
        static async Task DownloadFileAsync(string fileUrl, string downloadPath)
        {
            using (HttpClient client = new HttpClient())
            {
                using (var response = await client.GetAsync(fileUrl))
                {
                    response.EnsureSuccessStatusCode();
                    await using (var fs = new FileStream(downloadPath, FileMode.CreateNew))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                }
            }
        }

        // Method to extract the ZIP file
        static void ExtractZipFile(string zipPath, string extractPath)
        {
            ZipFile.ExtractToDirectory(zipPath, extractPath);
        }
    }
}
