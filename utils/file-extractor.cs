using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

public class FileExtractor
{
    // Method to extract the ZIP or TAR file
    public static void ExtractZipFile(string archivePath, string extractPath)
    {
        // Determine the file extension
        string extension = Path.GetExtension(archivePath).ToLowerInvariant();

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Use the tar command for TAR files on Linux
            if (extension == ".tar" || extension == ".gz" || extension == ".tgz")
            {
                ExtractUsingTarCommand(archivePath, extractPath);
            }
            else if (extension == ".zip")
            {
                ExtractUsingUnzipCommand(archivePath, extractPath);
            }
            else
            {
                throw new NotSupportedException($"Unsupported archive format: {extension}");
            }
        }
        else
        {
            // Use the built-in .NET method for ZIP files on Windows
            if (extension == ".zip")
            {
                ZipFile.ExtractToDirectory(archivePath, extractPath);
            }
            else
            {
                throw new NotSupportedException($"Unsupported archive format: {extension}");
            }
        }
    }

    // Method to extract the ZIP file using the Linux 'unzip' command
    private static void ExtractUsingUnzipCommand(string zipPath, string extractPath)
    {
        try
        {
            // Ensure the extraction directory exists
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            // Run the 'unzip' command on Linux
            Process process = new Process();
            process.StartInfo.FileName = "unzip";
            process.StartInfo.Arguments = $"\"{zipPath}\" -d \"{extractPath}\"";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            // Capture the output of the unzip process
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Unzip failed: {error}");
            }

            Console.WriteLine("Unzip successful: " + output);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during unzip: {ex.Message}");
            throw;
        }
    }

    // Method to extract the TAR file using the 'tar' command
    private static void ExtractUsingTarCommand(string tarPath, string extractPath)
    {
        try
        {
            // Ensure the extraction directory exists
            if (!Directory.Exists(extractPath))
            {
                Directory.CreateDirectory(extractPath);
            }

            // Run the 'tar' command on Linux
            Process process = new Process();
            process.StartInfo.FileName = "tar";
            process.StartInfo.Arguments = $"-xf \"{tarPath}\" -C \"{extractPath}\""; // -C option for directory
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;

            process.Start();

            // Capture the output of the tar process
            string output = process.StandardOutput.ReadToEnd();
            string error = process.StandardError.ReadToEnd();

            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception($"Tar extraction failed: {error}");
            }

            Console.WriteLine("Tar extraction successful: " + output);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred during tar extraction: {ex.Message}");
            throw;
        }
    }
}
