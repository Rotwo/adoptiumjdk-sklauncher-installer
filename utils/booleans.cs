using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adoptiumjdk_sklauncher_installer.utils
{
    internal class booleans
    {
        // Method to check if the Java JDK is already installed
        public static bool IsJavaInstalled(string extractPath)
        {
            // Define the expected installation path
            string jdkPath = Path.Combine(extractPath, "jdk-21.0.4+7"); // Adjust based on how you extract and structure your files

            // Check if the JDK directory exists and contains expected files
            return Directory.Exists(jdkPath) &&
                   File.Exists(Path.Combine(jdkPath, "bin", "java.exe")); // Adjust for the expected files you need
        }
    }
}
