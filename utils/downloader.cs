using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace adoptiumjdk_sklauncher_installer.utils
{
    internal class Downloader
    {
        public async Task Download(DownloadParams downloadParams)
        {
            DownloadUrl determinedDownloadUrl;

            // For universal downloads
            if (downloadParams.universal)
            {
                determinedDownloadUrl = downloadParams.downloadUrl[0];
            }
            else
            {
                // Determine the OS platform
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Console.WriteLine("Detected OS: Windows");
                    determinedDownloadUrl = Array.Find(downloadParams.downloadUrl, _downloadUrl => _downloadUrl.os == OSPlatform.Windows);
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Console.WriteLine("Detected OS: Linux");
                    determinedDownloadUrl = Array.Find(downloadParams.downloadUrl, _downloadUrl => _downloadUrl.os == OSPlatform.Linux);
                }
                else
                {
                    // Fallback: use the first download URL as default if the OS isn't Windows or Linux
                    Console.WriteLine("OS not detected or unsupported. Using default download URL.");
                    determinedDownloadUrl = downloadParams.downloadUrl[0];
                }
            }

            // Ensure we have a valid URL to download
            if (determinedDownloadUrl == null || string.IsNullOrEmpty(determinedDownloadUrl.url))
            {
                throw new Exception("No valid download URL found for the current OS.");
            }

            try
            {
                Console.WriteLine($"Downloading {downloadParams.debug_downloadTitle} from {determinedDownloadUrl.url}.");
                await DownloadFileAsync(determinedDownloadUrl.url, determinedDownloadUrl.fileName);
                Console.WriteLine("Download complete.");

                Console.WriteLine($"Installing {downloadParams.debug_downloadTitle}");
                FileExtractor.ExtractZipFile(determinedDownloadUrl.fileName, downloadParams.extractPath);
                Console.WriteLine("Installation complete.");

                // Optional: Delete the downloaded file after extraction
                File.Delete(determinedDownloadUrl.fileName);
                Console.WriteLine($"{downloadParams.debug_downloadTitle} clean-up completed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred during download or installation: {ex.Message}");
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
    }
}
