using adoptiumjdk_sklauncher_installer.utils;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Threading.Tasks;

public class DownloadParams
{
    public string debug_downloadTitle { get; set; }
    public string downloadUrl { get; set; }
    public string downloadFileName { get; set; }
    public string extractPath { get; set; }
}

class Program
{
    static async Task Main(string[] args)
    {
        var initializer = new Initializer();
        var downloader = new Downloader();

        var adoptiumDownloadParams = new DownloadParams
        {
            debug_downloadTitle = "Java JDK21",
            downloadUrl = "https://github.com/adoptium/temurin21-binaries/releases/download/jdk-21.0.4%2B7/OpenJDK21U-jdk_x64_windows_hotspot_21.0.4_7.zip",
            downloadFileName = "OpenJDK21U-jdk_x64_windows_hotspot_21.0.4_7.zip",
            extractPath = "runtime"
        };
        var repoDownloadParams = new DownloadParams
        {
            debug_downloadTitle = "Assets",
            downloadUrl = "https://gitlab.com/Rotwo/sklauncher-adoptiumjdk-script/-/archive/main/sklauncher-adoptiumjdk-script-main.zip",
            downloadFileName = "sklauncher-adoptium-script-gitlab-repo.zip",
            extractPath = "core"
        };

        try
        {
            await downloader.Download(adoptiumDownloadParams);
            await downloader.Download(repoDownloadParams);
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
