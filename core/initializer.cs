using System;
using System.Diagnostics;
using System.Threading.Tasks;

internal class Initializer
{
    public async Task StartLauncher()
    {
        string javaPath = @"/runtime/bin/java.exe";
        string launcherPath = @"/core/sklauncher-adoptiumjdk-script-main/static/SKLauncher.jar";
        string args = $"-jar \"{launcherPath}\""; // Use quotes to handle spaces in paths

        // Create a new process
        var processStartInfo = new ProcessStartInfo
        {
            FileName = javaPath, // Path to the executable or jar
            Arguments = args, // Arguments to pass
            RedirectStandardOutput = true, // Optional: redirect output
            RedirectStandardError = true, // Optional: redirect error output
            UseShellExecute = false, // Required for redirection
            CreateNoWindow = true // Optional: no window
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
