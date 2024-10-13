using adoptiumjdk_sklauncher_installer.utils;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

public class DownloadParams
{
    public string debug_downloadTitle { get; set; }
    public DownloadUrl[] downloadUrl { get; set; }
    public string extractPath { get; set; }
    public bool universal { get; set; }
}

public class DownloadUrl
{
    public OSPlatform os { get; set; }
    public string url { get; set; }
    public string fileName { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        var initializer = new Initializer();
        var downloader = new Downloader();

        // Create instances of DownloadUrls for two different OS platforms
        DownloadUrl adoptiumWindowsDownload = new DownloadUrl
        {
            os = OSPlatform.Linux,
            url = "https://github.com/adoptium/temurin21-binaries/releases/download/jdk-21.0.4%2B7/OpenJDK21U-jre_x64_linux_hotspot_21.0.4_7.tar.gz",
            fileName = "OpenJDK21U-jdk_x64_linux_hotspot_21.0.4_7.tar.gz"
        };

        DownloadUrl adoptiumLinuxDownload = new DownloadUrl
        {
            os = OSPlatform.Windows,
            url = "https://github.com/adoptium/temurin21-binaries/releases/download/jdk-21.0.4%2B7/OpenJDK21U-jre_x64_windows_hotspot_21.0.4_7.zip",
            fileName = "OpenJDK21U-jdk_x64_windows_hotspot_21.0.4_7.zip"
        };

        var adoptiumDownloadParams = new DownloadParams
        {
            debug_downloadTitle = "JDK 21 (Minecraft Runtime)",
            downloadUrl = new DownloadUrl[] {
                adoptiumWindowsDownload,
                adoptiumLinuxDownload
            },
            extractPath = "runtime",
            universal = false
        };
        var repoDownloadParams = new DownloadParams
        {
            debug_downloadTitle = "Assets",
            downloadUrl = new DownloadUrl[] {
                new DownloadUrl
                {
                    os = OSPlatform.Windows,
                    url = "https://gitlab.com/Rotwo/sklauncher-adoptiumjdk-script/-/archive/main/sklauncher-adoptiumjdk-script-main.zip",
                    fileName = "sklauncher-adpotiumjdk-script-repo.zip"
                }
            },
            extractPath = "core",
            universal = true
        };

        try
        {
            // Check if Java JDK is already installed
            if (!booleans.IsJavaInstalled(adoptiumDownloadParams.extractPath))
            {
                await downloader.Download(adoptiumDownloadParams);
                await downloader.Download(repoDownloadParams);
            }
            else
            {
                Console.WriteLine("Java JDK is already installed, skipping download.");
                await downloader.Download(repoDownloadParams);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }

        await initializer.StartLauncher();

        // Optionally keep the program alive or handle other tasks.
        Console.WriteLine("Press any key to exit...");
        Console.ReadKey();
    }
}
