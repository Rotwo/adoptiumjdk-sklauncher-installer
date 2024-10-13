using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

internal class Initializer
{
    public async Task StartLauncher()
    {
        // Get the program directory
        string programDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Paths (different for Windows and Linux)
        string javaPath;
        string launcherPath;

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            // Windows-specific paths
            javaPath = @"runtime\jdk-21.0.4+7-jre\bin\java.exe"; // Windows uses backslashes for paths
            launcherPath = @"core\sklauncher-adoptiumjdk-script-main\static\SKlauncher-3.2.10.jar";
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            // Linux-specific paths
            javaPath = "runtime/jdk-21.0.4+7-jre/bin/java"; // Linux uses forward slashes for paths
            launcherPath = "core/sklauncher-adoptiumjdk-script-main/static/SKlauncher-3.2.10.jar";
        }
        else
        {
            Console.WriteLine("Unsupported platform");
            return;
        }

        // Combine the program directory with the relative paths
        string fullJavaPath = Path.Combine(programDirectory, javaPath);
        string fullLauncherPath = Path.Combine(programDirectory, launcherPath);

        // Set arguments
        string javaArgs = $"-jar \"{fullLauncherPath}\""; // Use quotes to handle spaces in paths

        // Create a new process
        var processStartInfo = new ProcessStartInfo
        {
            FileName = fullJavaPath, // Path to the Java executable
            Arguments = javaArgs,    // Arguments for the process
            RedirectStandardOutput = true, // Optional: redirect output
            RedirectStandardError = true,  // Optional: redirect error output
            UseShellExecute = false,       // Required for redirection
            CreateNoWindow = true          // Optional: run without creating a window
        };

        try
        {
            // Start the process
            using (var process = Process.Start(processStartInfo))
            {
                // Optional: read the output asynchronously
                process.OutputDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Console.WriteLine($"Output: {e.Data}");
                    }
                };
                process.ErrorDataReceived += (sender, e) =>
                {
                    if (e.Data != null)
                    {
                        Console.WriteLine($"Error: {e.Data}");
                    }
                };

                // Start async reading
                process.BeginOutputReadLine();
                process.BeginErrorReadLine();

                // Wait for the process to exit asynchronously
                await process.WaitForExitAsync();

                // Output exit code
                Console.WriteLine($"Exit Code: {process.ExitCode}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
